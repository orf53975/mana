using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;

namespace mana.Server.Game.Handler
{
    [MessageForwarding("Battle.PlayerMove")]
    class PlayerMove : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            //if (token.UserData is GamePlayer gamePlayer)
            //{
            //    token.GetServer<GameServer>().ForwardingToBattleServer(gamePlayer, packet);
            //}
            //TODO test
            packet.SetAttach(token.uid);
            token.GetServer<GameServer>().ForwardingToBattleServer(0, packet);
        }
    }
}
