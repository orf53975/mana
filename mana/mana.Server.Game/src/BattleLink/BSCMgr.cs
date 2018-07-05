using mana.Foundation;
using System;

namespace mana.Server.Game.BattleLink
{
    class BSCMgr
    {
        internal readonly BSClient[] clients;

        internal readonly GameServer server;

        public BSCMgr(GameServer gameServer, string[] bsAddrs)
        {
            this.server = gameServer;
            var num = bsAddrs != null ? bsAddrs.Length : 0;
            clients = new BSClient[num];
            for (int i = 0; i < num; i++)
            {
                clients[i] = new BSClient(this, bsAddrs[i]);
            }
        }

        internal void StartConnect()
        {
            for (int i = 0; i < clients.Length; i++)
            {
                clients[i].StartConnect();
            }
        }

        internal void Send(int clientId, Packet msg)
        {
            clients[clientId].SendPacket(msg);
        }

        internal void Update(int deltaTime)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                clients[i].Update(deltaTime);
            }
        }
    }
}
