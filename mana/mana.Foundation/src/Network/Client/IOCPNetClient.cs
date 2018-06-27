using System;
using System.Net;
using System.Net.Sockets;

namespace mana.Foundation.Network.Client
{
    public class IOCPNetClient : AbstractNetClient
    {
        readonly PacketRcver packetRcver = new PacketRcver();

        readonly PacketSnder packetSnder = new PacketSnder();

        readonly SocketAsyncEventArgs rcvEventArg;

        readonly SocketAsyncEventArgs sndEventArg;

        readonly int SendBufferOffset;

        readonly int SendBufferLimit;

        Socket _socket = null;

        float lastRcvTime = 0.0f;

        float lastSndTime = 0.0f;

        public int port
        {
            get;
            private set;
        }

        public IPAddress remote
        {
            get;
            private set;
        }

        public bool isConnected
        {
            get
            {
                return _socket != null && _socket.Connected;
            }
        }

        public IOCPNetClient(int rcvBuffSize = 2048, int sndBuffSize = 1024)
        {
            rcvEventArg = new SocketAsyncEventArgs();
            rcvEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(RcvCompleted);
            rcvEventArg.UserToken = this;
            rcvEventArg.SetBuffer(new byte[rcvBuffSize], 0, rcvBuffSize);

            sndEventArg = new SocketAsyncEventArgs();
            sndEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(SndCompleted);
            sndEventArg.UserToken = this;
            sndEventArg.SetBuffer(new byte[sndBuffSize], 0, sndBuffSize);
            SendBufferOffset = sndEventArg.Offset;
            SendBufferLimit = SendBufferOffset + sndEventArg.Count;
        }

        #region <<about rcv>>

        void RcvCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                this.ProcessRcved(e);
            }
            else
            {
                Logger.Error("The last operation completed on the socket was not a rcv.");
                this.OnNetError();
            }
        }

        void ProcessRcved(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                try
                {
                    this.OnRcv(e.Buffer, e.Offset, e.BytesTransferred);
                    if (!_socket.ReceiveAsync(e))
                    {
                        this.ProcessRcved(e);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                    this.OnNetError();
                }
            }
            else
            {
                this.OnNetError();
            }
        }

        void OnRcv(byte[] buffer, int offset, int count)
        {
            packetRcver.PushData(buffer, offset, count);
            var p = packetRcver.Build();
            while (p != null)
            {
                this.OnRecivedPacket(p);
                p = packetRcver.Build();
            }
            lastRcvTime = Environment.TickCount;
        }

        #endregion

        #region <<about snd>>

        void SndCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Send)
            {
                this.ProcessSnded(e);
            }
            else
            {
                Logger.Error("The last operation completed on the socket was not a snd.");
                this.OnNetError();
            }
        }


        void ProcessSnded(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                try
                {
                    this.DoSending(e);
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                    this.OnNetError();
                }
            }
            else
            {
                this.OnNetError();
            }
        }

        void DoSending(SocketAsyncEventArgs e)
        {
            var offset = SendBufferOffset;
            while (offset < SendBufferLimit && packetSnder.HasSendingData)
            {
                packetSnder.WriteTo(e.Buffer, ref offset, SendBufferLimit);
            }
            var nSndBytes = offset - SendBufferOffset;
            if (nSndBytes != 0)
            {
                e.SetBuffer(SendBufferOffset, nSndBytes);
                var willRaiseEvent = _socket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessSnded(e);
                }
            }
        }

        #endregion

        public override void Connect(string ip, ushort port, Action<bool, Exception> callback)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SendTimeout = 3000;

            var saea = new SocketAsyncEventArgs();
            saea.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            saea.Completed += new EventHandler<SocketAsyncEventArgs>(AsyncConnected);
            saea.UserToken = this;
            _socket.ConnectAsync(saea);
        }

        static void AsyncConnected(object sender, SocketAsyncEventArgs e)
        {
            if(e.SocketError == SocketError.Success)
            {

            }
            throw new NotImplementedException();
        }

        public override void Disconnect()
        {
            try
            {
                if (_socket != null && _socket.Connected)
                {
                    _socket.Close();
                }
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
            finally
            {
                _socket = null;
            }
        }

        public override void SendPacket(Packet p)
        {
            //if (state != kStateLink)
            //{
            //    Logger.Warning("UserToken.Send Failed! state = {0} error!", state);
            //    return;
            //}
            //var bInSending = packetSnder.HasSendingData;
            //packetSnder.Push(p);
            //if (!bInSending)
            //{
            //    DoSending(sndEventArg);
            //}
            throw new NotImplementedException();
        }
    }
}
