using mana.Foundation;
using mana.Foundation.Network.Client;
using System;
using System.Net;

namespace mana.Server.Game.BattleLink
{
    class BSClient
    {
        readonly IPEndPoint address;

        readonly NetClient  channel;

        readonly BSCMgr     manager;

        public BSClient(BSCMgr mgr, string addr)
        {
            this.manager = mgr;
            this.channel = new NetClientIOCP(5000, 2048, 1024, true);
            this.address = Utils.GetIPEndPoint(addr);
            this.InitPacketListener();
        }

        internal void StartConnect()
        {
            if (channel.Connected)
            {
                return;
            }
            channel.Connect(address, (sucess) =>
            {
                if (sucess)
                {
                    BindToken();
                }
                else
                {
                    StartConnect();
                }
            });
        }

        void InitPacketListener()
        {
            channel.AddPacketListener("Battle.Sync", (p) =>
            {
                var uid = p.msgToken;
                p.SetMsgToken(null);
                manager.server.Send(uid , p);
            });
        }

        void BindToken()
        {
            channel.Request<AccountInfo, Result>("Connector.BindToken",
                (acc) =>
                {
                    acc.username = Config.ServerSetting.name;
                    acc.password = "12345678";
                },
                (res) =>
                {
                    Logger.Print("BindToken:{0}", res.code);
                });
        }

        internal void SendPacket(Packet msg)
        {
            channel.SendPacket(msg);
        }

        internal void Update(int deltaTime)
        {
            channel.Update(deltaTime);
        }

    }
}
