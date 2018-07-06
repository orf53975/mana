using mana.Foundation;
using mana.Foundation.Network.Sever;
using xxd.battle;
using xxd.game;

namespace mana.Server.Battle.Handlers.Request
{
    [MessageRequest("Battle.CreateDungeon", typeof(CreateDungeon), typeof(Result))]
    class CreateDungeonRequest : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            var bci = BattleCreateDataHelper.Gen(packet.TryGet<CreateDungeon>());
            var scn = token.GetServer<BattleServer>().CreateBattle(bci, token.uid);
            token.SendResponse<Result>("Battle.CreateDungeon", packet.msgRequestId, (res) =>
            {
                res.code = Result.Code.sucess;
                res.info = scn.uid;
            });
        }
    }
}
