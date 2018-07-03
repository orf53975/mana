using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;

namespace mana.Server.Battle.src.Handlers
{
    [MessageNotify("Battle.PlayerMove", typeof(xxd.sync.opration.MoveRequest))]
    class PlayerMove : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            throw new NotImplementedException();
        }
    }
}
