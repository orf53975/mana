using mana.Foundation;
using mana.Foundation.Network.Client;
using mana.Network.TCP.Client;
using xxd.game;

namespace mana.Test.Client
{
    public class TestClient
    {
        public readonly NetClient Channel;

        readonly int clientId;

        readonly string ip;

        readonly ushort port;

        public TestClient(int id , string ip, ushort port)
        {
            this.clientId = id;
            this.ip = ip;
            this.port = port;

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
                    acc.username = "TestUser";
                    acc.password = "12345678";
                },
                (res) =>
                {
                    Logger.Print(res.ToFormatString(""));
                });
        }

        internal void DoTest()
        {
            //Channel.Notify<MoveRequest>("Battle.PlayerMove",
            //    (mr) =>
            //    {
            //        mr.unitId = 1001;
            //        mr.x = 10;
            //        mr.y = 10;
            //        mr.z = 10;
            //        mr.type = 0;
            //    });

            Channel.Request<ChallengeDungeon, Result>("Game.Dungeon.Challenge",
                (req) =>
                {
                    req.dungeonTmpl = "mszc_1";
                    req.difficulty = 0;
                },
                (rt) =>
                {
                    Logger.Print("Game.Dungeon.Challenge:{0}", rt);
                });
        }

    }
}
