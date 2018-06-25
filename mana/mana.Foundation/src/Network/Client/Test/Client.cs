using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestClient
{
    public class CustomClient
    {
        readonly PacketRcver packetRcver = new PacketRcver((p) =>
        {
            Console.WriteLine();
            Console.Write("recv> opcode = {0} , info = {1}", p.opcode , p.decodeString());
            Console.WriteLine();
        });

        public TcpClient _client;

        public int port;

        public IPAddress remote;

        public CustomClient(int port, IPAddress remote)
        {

            this.port = port;
            this.remote = remote;
        }

        public void connect()
        {
            this._client = new TcpClient();
            _client.Connect(remote, port);
        }

        public bool isConnected
        {
            get
            {
                return _client.Connected;
            }
        }

        public void disconnect()
        {
            _client.Close();
        }

        public void send(string msg)
        {
            byte[] data = Encoding.Default.GetBytes(msg);
            _client.GetStream().Write(data, 0, data.Length);
        }

        public void send(byte[] data)
        {
            _client.GetStream().Write(data, 0, data.Length);
        }

        public void send(Packet p)
        {
            using (var wb = WriteonlyBuffer.Pool.Get())
            {
                p.writeTo(wb);
                _client.GetStream().Write(wb.data, 0, wb.size);
            }
        }

        readonly byte[] buffer = new byte[1024];

        public void startRecive()
        {
            new Thread(() =>
            {
                Console.WriteLine("Start Recive");
                while (true)
                {
                    recive();
                    Thread.Sleep(100);
                }
            }).Start();

            Thread.Sleep(50);
        }

        void recive()
        {
            var stream = _client.GetStream();
            if (stream.CanRead && stream.DataAvailable)
            {
                var len = _client.GetStream().Read(buffer, 0, buffer.Length);
                packetRcver.pushData(buffer, 0, len);
            }
        }

    }
}
