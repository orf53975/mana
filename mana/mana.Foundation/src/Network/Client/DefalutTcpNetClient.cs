using System;
using System.Net;
using System.Net.Sockets;

namespace mana.Foundation.Network.Client
{
    public class DefalutTcpNetClient : AbstractNetClient
    {
        private TcpClient _client;

        public int port { get; private set; }

        public IPAddress remote { get; private set; }

        public bool isConnected
        {
            get
            {
                return _client != null && _client.Connected;
            }
        }

        public Action<bool, Exception> connCallback;

        public override void Connect(string ip, ushort port, Action<bool, Exception> callback)
        {
            try
            {
                this.remote = IPAddress.Parse(ip);
                this.port = port;
                this.connCallback = callback;
                this._client = new TcpClient();
                this._client.BeginConnect(remote, port, ConnectCallback, this);
            }
            catch (Exception ex)
            {
                if (callback != null) { callback.Invoke(false, ex); }
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            var nc = ar.AsyncState as DefalutTcpNetClient;
            try
            {
                nc._client.EndConnect(ar);
                if (ar.IsCompleted)
                {
                    nc.connCallback(true, null);
                }
                else
                {
                    nc.connCallback(false, null);
                }
            }
            catch (SocketException ex)
            {
                nc.connCallback(false , ex);
            }
        }


        public override void Disconnect()
        {
            if(_client != null)
            {
                _client.Close();
                _client = null;
            }
        }

        protected override int ChannelPull(byte[] buffer, int offset, int size)
        {
            var stream = _client.GetStream();
            if (stream.CanRead && stream.DataAvailable)
            {
                return _client.GetStream().Read(buffer, offset, size);
            }
            else
            {
                return 0;
            }
        }

        protected override int ChannelPush(byte[] buffer, int offset, int size)
        {
            var stream = _client.GetStream();
            if (stream.CanWrite)
            {
                _client.GetStream().Write(buffer, offset, size);
                return size;
            }
            else
            {
                return 0;
            }
        }
    }
}
