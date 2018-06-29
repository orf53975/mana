using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;

namespace mana.Server.Battle.src.Handlers
{
    [MessageNotify("Battle.PlayerCast", typeof(xxd.sync.opration.CastRequest))]
    class PlayerCast : IMessageHandler
    {
        public void Process(UserToken token, Packet msg)
        {
            throw new NotImplementedException();
        }
    }
}
