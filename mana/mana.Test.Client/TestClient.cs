using mana.Foundation;
using mana.Foundation.Network.Client;
using System.Threading;

namespace mana.Test.Client
{
    public class TestClient
    {
        public readonly NetClient mNetChannel;

        readonly Thread mThread;

        readonly int clientId;

        readonly string ip;

        readonly ushort port;

        public TestClient(int id , string ip, ushort port)
        {
            this.clientId = id;
            this.ip = ip;
            this.port = port;
            mThread = new Thread(UpdateProc);
            mThread.Start();
            mNetChannel = new NetClientDefault(true, 20000);
            this.InitChanne();
        }

        private void InitChanne()
        {
            mNetChannel.AddPushListener<Protocol>("Connector.Protocol", (protocol) =>
            {
                Logger.Print(Protocol.Instance.ToFormatString(""));
            });
            mNetChannel.AddPushListener<Heartbeat>("Connector.Pong", (protocol) =>
            {
                Logger.Print(protocol.ToFormatString(""));
            });
            mNetChannel.Connect(ip, port, (sucess) =>
            {
                Logger.Print("connected {0}!", sucess ? "sucessed" : "failed");
            });
        }

        private void UpdateProc()
        {
            while (true)
            {
                if(mNetChannel != null)
                {
                    mNetChannel.Update(20);
                }
                Thread.Sleep(20);
            }
        }
    }
}
