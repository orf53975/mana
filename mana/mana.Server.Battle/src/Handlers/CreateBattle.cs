using mana.Foundation;
using mana.Foundation.Network.Sever;
using xxd.sync;

namespace mana.Server.Battle.src.Handlers
{
    [MessageRequest("Battle.CreateBattle", typeof(BattleCreateData), typeof(Result))]
    class CreateBattle : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            var sev = token.GetServer<BattleServer>();
            var bcd = packet.TryGet<BattleCreateData>();
            sev.CreateBattle(bcd);
            token.SendResponse<Result>("Battle.CreateBattle", packet.msgRequestId, (res) =>
            {
                res.code = Result.Code.sucess;
            });
        }
    }
}
