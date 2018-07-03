using mana.Foundation;
using mana.Foundation.Network.Sever;
using xxd.sync;

namespace mana.Server.Battle.src.Handlers
{
    [MessageRequest("Battle.CreateBattle", typeof(BattleCreateData), typeof(Response))]
    class CreateBattle : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            var sev = token.GetServer<BattleServer>();
            var bcd = packet.TryGet<BattleCreateData>();
            sev.CreateBattle(bcd);
            token.SendResponse<Response>("Battle.CreateBattle", packet.msgRequestId, (res) =>
            {
                res.code = Response.Code.sucess;
            });
        }
    }
}
