using mana.Foundation;
using mana.Foundation.Network.Client;
using System;
using System.Threading;
using xxd.sync.opration;

namespace mana.Test.Client
{
    public class TestClient
    {
        public readonly NetClient Channel;

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

            Channel = new NetClientDefault(true, 20000);
            this.InitChannel();
        }

        private void InitChannel()
        {
            Channel.AddPushListener<Protocol>("Connector.Protocol", (protocol) =>
            {
                Logger.Print(Protocol.Instance.ToFormatString(""));
            });
            Channel.AddPushListener<Heartbeat>("Connector.Pong", (protocol) =>
            {
                Logger.Print(protocol.ToFormatString(""));
            });
        }

        public void StartConnect()
        {
            Channel.Connect(ip, port, (sucess) =>
            {
                Logger.Print("connected {0}!", sucess ? "sucessed" : "failed");
                if (sucess)
                {
                    BindToken();
                }
            });
        }

        void BindToken()
        {
            Channel.Request<AccountInfo, Result>("Connector.BindToken",
                (acc) =>
                {
                    acc.username = "testUser";
                    acc.password = "12345678";
                },
                (res) =>
                {
                    Logger.Print(res.ToFormatString(""));
                });
        }


        private void UpdateProc()
        {
            while (true)
            {
                if(Channel != null)
                {
                    Channel.Update(20);
                }
                Thread.Sleep(20);
            }
        }

        internal void DoTest()
        {
            Channel.Notify<MoveRequest>("Battle.PlayerMove",
                (mr) =>
                {
                    mr.unitId = 1001;
                    mr.x = 10;
                    mr.y = 10;
                    mr.z = 10;
                    mr.type = 0;
                });
        }

    }
}
