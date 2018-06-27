using System;
using System.Net;
using System.Net.Sockets;

namespace mana.Foundation.Network.Client
{
    public class DefalutNetClient : AbstractNetClient
    {
        readonly PacketRcver packetRcver = new PacketRcver();

        readonly PacketSnder packetSnder = new PacketSnder();

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

        public DefalutNetClient()
        {
        }

        #region <<about rcv>>
        static int __ChannelPull(DefalutNetClient nc, byte[] data, int offset, int count)
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
            var count = packetRcver.PushData(this, __ChannelPull);
            while (count > 0)
            {
                var p = packetRcver.Build();
                while (p != null)
                {
                    this.OnRecivedPacket(p);
                    p = packetRcver.Build();
                }
                count = packetRcver.PushData(this, __ChannelPull);
            }
        }

        #endregion

        #region <<about rcv>>
        public void DoSnding()
        {
            var count = packetSnder.WriteTo(this, __ChannelPush);
            while (count > 0)
            {
                count = packetSnder.WriteTo(this, __ChannelPush);
            }
        }

        static int __ChannelPush(DefalutNetClient nc, byte[] data, int offset, int count)
        {
            return nc.ChannelPush(data, offset, count);
        }

        private int ChannelPush(byte[] buffer, int offset, int size)
        {
            return _socket.Send(buffer, offset, size, SocketFlags.None);
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
            packetSnder.Push(p);
        }
    }
}
