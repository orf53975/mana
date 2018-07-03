using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;

namespace mana.Server.Game.Handler
{
    [MessageForwarding("Battle.PlayerCast")]
    class PlayerCast : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            if (token.UserData is GamePlayer gamePlayer)
            {
                token.GetServer<GameServer>().ForwardingToBattleServer(gamePlayer, packet);
            }
        }
    }
}
