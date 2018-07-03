using mana.Foundation;
using System;

namespace mana.Server.Game.BattleLink
{
    class BSCMgr
    {
        private readonly BSClient[] clients;

        public BSCMgr(string[] battleServerAddrs)
        {
            var num = battleServerAddrs != null ? battleServerAddrs.Length : 0;
            clients = new BSClient[num];
            for (int i = 0; i < num; i++)
            {
                clients[i] = new BSClient(battleServerAddrs[i]);
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
            clients[clientId].Channel.SendPacket(msg);
        }
    }
}
