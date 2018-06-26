using mana.Foundation;
using mana.Foundation.Network.Client;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace mana.Test.Client
{
    public class CustomNetChannel : NetChannel
    {
        readonly PacketRcver packetRcver = new PacketRcver();

        readonly PacketSnder packetSnder = new PacketSnder();

        public TcpClient _client;

        public int port;

        public IPAddress remote;

        public bool isConnected
        {
            get
            {
                return _client.Connected;
            }
        }

        public void StartConnect(string ip, ushort port, Action<bool, Exception> callback)
        {
            this.remote = IPAddress.Parse(ip);
            this.port = port;
            this._client = new TcpClient();
            this._client.Connect(remote, port);
            Trace.TraceInformation("Connect Sucess!");
        }

        public void Send(Packet p)
        {
            packetSnder.Push(p);
        }

        public void AddListener(Action<Packet> p)
        {
            this.OnRecived = p;
        }

        public void Disconnect()
        {
            _client.Close();
        }

        readonly byte[] snd_buffer = new byte[1024];

        private void DoSnding()
        {
            if (packetSnder.HasSendingData)
            {
                var bytesCount = 0;
                packetSnder.WriteTo(snd_buffer, ref bytesCount, snd_buffer.Length);
                _client.GetStream().Write(snd_buffer, 0, bytesCount);
            }
        }

        private Action<Packet> OnRecived = null;
        readonly byte[] buffer = new byte[1024];
        private void DoRcving()
        {
            var stream = _client.GetStream();
            if (stream.CanRead && stream.DataAvailable)
            {
                var len = _client.GetStream().Read(buffer, 0, buffer.Length);
                packetRcver.PushData(buffer, 0, len);

                var p = packetRcver.Build();
                if (p != null)
                {
                    Trace.TraceInformation("recv> route = {0}", p.msgRoute);
                    OnRecived?.Invoke(p);
                }
            }
        }

        public void StartThread()
        {
            new Thread(() =>
            {
                Trace.TraceInformation("Start NetWork");
                while (true)
                {
                    if(_client != null && _client.Connected)
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
