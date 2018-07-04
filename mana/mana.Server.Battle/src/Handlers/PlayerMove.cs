using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;
using xxd.sync;

namespace mana.Server.Battle.src.Handlers
{
    [MessageNotify("Battle.PlayerMove", typeof(xxd.sync.opration.MoveRequest))]
    class PlayerMove : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            //TODO test
            token.SendPush<BattleSync>("Battle.Sync", (bs) =>
            {
                bs.actions = new DataObject[3];
                bs.actions[0] = ObjectCache.Get<AddUnit>();
                bs.actions[1] = ObjectCache.Get<RemoveUnit>();
                bs.actions[2] = ObjectCache.Get<BuffData>();
            } , packet.msgToken);
            //throw new NotImplementedException();
        }
    }
}
