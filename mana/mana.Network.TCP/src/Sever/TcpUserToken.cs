using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using mana.Foundation;
using mana.Foundation.Network.Server;

namespace mana.Network.TCP.Sever
{
    public sealed class TCPUserToken : UserToken
    {
        internal const int kDefaultBufferSize = 1024;

        readonly SocketAsyncEventArgs rcvEventArg;

        readonly SocketAsyncEventArgs sndEventArg;

        readonly int SendBufferOffset;

        readonly int SendBufferLimit;

        private Socket socket;

        public override EndPoint Address
        {
            get
            {
                return socket != null ? socket.RemoteEndPoint : null;
            }
        }

        public TCPUserToken(BufferManager bufferManager, AbstractServer server) : base(server)
        {
            rcvEventArg = new SocketAsyncEventArgs();
            rcvEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(RcvCompleted);
            rcvEventArg.UserToken = this;
            if (bufferManager == null || bufferManager.SetBuffer(rcvEventArg))
            {
                rcvEventArg.SetBuffer(new byte[kDefaultBufferSize], 0, kDefaultBufferSize);
            }
            sndEventArg = new SocketAsyncEventArgs();
            sndEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(SndCompleted);
            sndEventArg.UserToken = this;
            if (bufferManager == null || bufferManager.SetBuffer(sndEventArg))
            {
                sndEventArg.SetBuffer(new byte[kDefaultBufferSize], 0, kDefaultBufferSize);
            }
            SendBufferOffset = sndEventArg.Offset;
            SendBufferLimit = SendBufferOffset + sndEventArg.Count;
        }


        internal void Init(Socket acceptSocket)
        {
            this.InitState();
            if (acceptSocket == null)
            {
                throw new NullReferenceException();
            }
            this.socket = acceptSocket;
            this.StartRcv();
        }

        #region <<rcv>>

        void StartRcv()
        {
            var willRaiseEvent = this.socket.ReceiveAsync(rcvEventArg);
            if (!willRaiseEvent)
            {
                ProcessRcved(rcvEventArg);
            }
        }

        void RcvCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                this.ProcessRcved(e);
            }
            else
            {
                Logger.Error("The last operation completed on the socket was not a rcv.");
                this.Release();
            }
        }


        void ProcessRcved(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                try
                {
                    this.OnRecived(e.Buffer, e.Offset, e.BytesTransferred);
                    if (!socket.ReceiveAsync(e))
                    {
                        this.ProcessRcved(e);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                    this.Release();
                }
            }
            else
            {
                this.Release();
            }
        }

        #endregion


        #region <<snd>>

        protected override void TryInvokeSendData()
        {
            if (Monitor.TryEnter(sndEventArg))
            {
                try
                {
                    DoSending(sndEventArg);
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }
                finally
                {
                    Monitor.Exit(sndEventArg);
                }
            }
        }

        void SndCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Send)
            {
                this.ProcessSnded(e);
            }
            else
            {
                Logger.Error("The last operation completed on the socket was not a snd.");
                this.Release();
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
                    this.Release();
                }
            }
            else
            {
                this.Release();
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
                var willRaiseEvent = socket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessSnded(e);
                }
            }
        }

        #endregion

        protected override void CloseChannel()
        {
            var addr = socket.RemoteEndPoint.ToString();
            try
            {
                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
            finally
            {
                socket.Close();
                this.server.OnSocketDisconnected(addr);
                socket = null;
            }
        }

    }
}