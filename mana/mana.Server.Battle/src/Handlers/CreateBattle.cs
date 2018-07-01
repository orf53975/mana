using mana.Foundation;
using mana.Foundation.Network.Sever;
using xxd.sync;

namespace mana.Server.Battle.src.Handlers
{
    [MessageRequest("Battle.CreateBattle", typeof(BattleCreateData), typeof(Response))]
    class CreateBattle : IMessageHandler
    {
        public void Process(UserToken token, Packet msg)
        {
            var sev = token.GetServer<BattleServer>();
            var bcd = msg.TryGet<BattleCreateData>();
            sev.CreateBattle(bcd);
            token.SendResponse<Response>("Battle.CreateBattle", msg.msgRequestId, (res) =>
            {
                res.code = Response.Code.sucess;
            });
        }
    }
}
