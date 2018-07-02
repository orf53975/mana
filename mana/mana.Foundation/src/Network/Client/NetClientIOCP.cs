using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace mana.Foundation.Network.Client
{
    public class NetClientIOCP : NetClient
    {
        readonly PacketRcver packetRcver = new PacketRcver();

        readonly PacketSnder packetSnder = new PacketSnder();

        readonly SocketAsyncEventArgs rcvEventArg;

        readonly SocketAsyncEventArgs sndEventArg;

        readonly int SendBufferOffset;

        readonly int SendBufferLimit;

        readonly Queue<Packet> rcvQue = new Queue<Packet>();

        readonly int mPingPongTimeout;

        readonly int mPingTimeSpan;

        readonly bool isImmediateMode;

        Socket _socket = null;

        int lastRcvTime = 0;

        int lastSndTime = 0;

        public override bool Connected
        {
            get
            {
                if (_socket != null)
                {
                    return _socket.Connected;
                }
                return false;
            }
        }

        public override EndPoint RemoteEndPoint
        {
            get
            {
                if (_socket != null)
                {
                    return _socket.RemoteEndPoint;
                }
                return null;
            }
        }

        public NetClientIOCP(int pingPongTimeout = 30 * 1000, int rcvBuffSize = 2048, int sndBuffSize = 1024 , bool immediateMode = false)
        {
            this.mPingPongTimeout = pingPongTimeout;
            this.mPingTimeSpan = pingPongTimeout >> 1;
            this.isImmediateMode = immediateMode;

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

        void StartRcv()
        {
            var willRaiseEvent = this._socket.ReceiveAsync(rcvEventArg);
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
                if(!isImmediateMode)
                {
                    lock (rcvQue)
                    {
                        rcvQue.Enqueue(p);
                    }
                }
                else
                {
                    OnPacketRecived(p);
                    p.ReleaseToPool();
                }
                p = packetRcver.Build();
            }
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

        public override void Connect(IPEndPoint ipep, Action<bool> callback)
        {
            if (ipep == null)
            {
                Logger.Error("IPEndPoint is null!");
                callback(false);
                return;
            }
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SendTimeout = 3000;

            var saea = new SocketAsyncEventArgs();
            saea.RemoteEndPoint = ipep;
            saea.Completed += new EventHandler<SocketAsyncEventArgs>(AsyncConnected);
            saea.UserToken = new KeyValuePair<NetClientIOCP, Action<bool>>(this, callback);
            var willRaiseEvent = _socket.ConnectAsync(saea);
            if (!willRaiseEvent)
            {
                callback(true);
            }
        }

        static void AsyncConnected(object sender, SocketAsyncEventArgs e)
        {
            var ut = (KeyValuePair<NetClientIOCP, Action<bool>>)e.UserToken;

            if (e.SocketError == SocketError.Success)
            {
                if (ut.Value != null)
                {
                    ut.Value.Invoke(true);
                }
                ut.Key.StartRcv();
            }
            else
            {
                Logger.Error("connect[{0}] failed! {1}", e.RemoteEndPoint, e.SocketError);
                if (ut.Value != null)
                {
                    ut.Value.Invoke(false);
                }
            }
        }

        public override void Disconnect()
        {
            try
            {
                if (_socket != null && _socket.Connected)
                {
                    _socket.Close();
                }
                packetRcver.Clear();
                packetSnder.Clear();
                lock(rcvQue)
                {
                    rcvQue.Clear();
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
            packetSnder.Push(p);
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
            lastSndTime = curTime;
        }

        private int curTime;
        public override void Update(int deltaTimeMs)
        {
            if (!Connected) { return; }
            curTime = curTime + deltaTimeMs;
            if (!isImmediateMode && Monitor.TryEnter(rcvQue))
            {
                try
                {
                    while (rcvQue.Count > 0)
                    {
                        var p = rcvQue.Dequeue();
                        OnPacketRecived(p);
                        p.ReleaseToPool();
                        lastRcvTime = curTime;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }
                finally
                {
                    Monitor.Exit(rcvQue);
                }
            }
            if (curTime - lastRcvTime > mPingPongTimeout)
            {
                this.OnHeartbeatTimeout();
            }
            else if (curTime - lastSndTime > mPingTimeSpan)
            {
                this.SendPingPacket();
            }
        }
    }
}
