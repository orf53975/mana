using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace mana.Foundation.Network.Client
{
    public class NetClientDefault : NetClient
    {
        readonly PacketRcver packetRcver = new PacketRcver();

        readonly PacketSnder packetSnder = new PacketSnder();

        Socket _socket = null;

        int lastRcvTime = 0;

        int lastSndTime = 0;

        readonly Thread mSendThread;

        readonly int mPingPongTimeout;

        readonly int mPingTimeSpan;

        public NetClientDefault(bool bEnableSendThread = true, int pingPongTimeout = 30 * 1000)
        {
            if(bEnableSendThread)
            {
                mSendThread = new Thread(SendProc);
                mSendThread.Start();
            }
            mPingPongTimeout = pingPongTimeout;
            mPingTimeSpan = pingPongTimeout >> 1;
        }

        private void SendProc()
        {
            while (!IsDisposed)
            {
                if (_socket != null && _socket.Connected)
                {
                    DoSnding();
                }
                Thread.Sleep(50);
            }
        }

        #region <<about rcv>>

        static int __ChannelPull(NetClientDefault nc, byte[] data, int offset, int count)
        {
            return nc.ChannelPull(data, offset, count);
        }

        private int ChannelPull(byte[] buffer, int offset, int size)
        {
            if (_socket.Available > 0)
            {
                return _socket.Receive(buffer, offset, size, SocketFlags.None);
            }
            else
            {
                return 0;
            }
        }

        public void DoRcving()
        {
            var count = packetRcver.PushData(_socket);   // packetRcver.PushData(this, __ChannelPull);
            while (count > 0)
            {
                var p = packetRcver.Build();
                while (p != null)
                {
                    this.OnPacketRecived(p);
                    p = packetRcver.Build();
                    lastRcvTime = curTime;
                }
                count = packetRcver.PushData(_socket);
            }
        }

        #endregion

        #region <<about rcv>>
        public void DoSnding()
        {
            var count = packetSnder.WriteTo(_socket);
            while (count > 0)
            {
                count = packetSnder.WriteTo(_socket);
            }
        }

        static int __ChannelPush(NetClientDefault nc, byte[] data, int offset, int count)
        {
            return nc.ChannelPush(data, offset, count);
        }

        private int ChannelPush(byte[] buffer, int offset, int size)
        {
            return _socket.Send(buffer, offset, size, SocketFlags.None);
        }
        #endregion

        public override EndPoint RemoteEndPoint
        {
            get
            {
                return _socket.RemoteEndPoint;
            }
        }

        public override bool Connected
        {
            get
            {
                return _socket != null && _socket.Connected;
            }
        }


        public override void Connect(string ip, ushort port, Action<bool> callback)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SendTimeout = 3000;

            var saea = new SocketAsyncEventArgs();
            saea.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            saea.Completed += new EventHandler<SocketAsyncEventArgs>(AsyncConnected);
            saea.UserToken = callback;
            _socket.ConnectAsync(saea);
        }

        static void AsyncConnected(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                if (e.UserToken != null)
                {
                    var callback = (Action<bool>)e.UserToken;
                    callback.Invoke(true);
                }
            }
            else
            {
                Logger.Error("connect failed! {0}", e.SocketError);
                if (e.UserToken != null)
                {
                    var callback = (Action<bool>)e.UserToken;
                    callback.Invoke(false);
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
            lastSndTime = curTime;
        }

        private int curTime;
        public override void Update(int deltaTimeMs)
        {
            if (!Connected) { return; }
            curTime = curTime + deltaTimeMs;
            if (_socket != null && _socket.Connected)
            {
                DoRcving();
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
