using mana.Foundation;
using mana.Foundation.Network.Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace mana.Test.Client
{
    public class CustomNetClient : AbstractNetClient
    {
        public TcpClient _client { get; private set; }

        public int port { get; private set; }

        public IPAddress remote { get; private set; }

        public bool isConnected
        {
            get
            {
                return _client != null && _client.Connected;
            }
        }
        public override void Connect(string ip, ushort port, Action<bool, Exception> callback)
        {
            this.remote = IPAddress.Parse(ip);
            this.port = port;
            this._client = new TcpClient();
            this._client.Connect(remote, port);
            Logger.Print("Connect Sucess!");
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
            return 0;
        }

        protected override int ChannelPush(byte[] buffer, int offset, int size)
        {
            var stream = _client.GetStream();
            if (stream.CanWrite)
            {
                _client.GetStream().Write(buffer, offset, size);
                return size;
            }
            return 0;
        }

        public void StartThread()
        {
            new Thread(() =>
            {
                Logger.Print("Start NetWork");
                while (true)
                {
                    if (_client != null && _client.Connected)
                    {
                        DoSnding();
                        DoRcving();
                    }
                    Thread.Sleep(100);
                }
            }).Start();
            Thread.Sleep(50);
        }

    }
}
