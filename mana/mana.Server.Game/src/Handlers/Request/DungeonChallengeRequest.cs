using mana.Foundation;
using mana.Foundation.Network.Sever;
using xxd.game;

namespace mana.Server.Game.Handler.Request
{

    [MessageRequest("Game.Dungeon.Challenge", typeof(ChallengeDungeon), typeof(Result))]
    class DungeonChallengeRequest : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            var server = token.GetServer<GameServer>();
            var player = token.UserData as GamePlayer;
            var requestId = packet.msgRequestId;

            var dungeonCreateData = GenDungeonCreateData(packet.TryGet<ChallengeDungeon>());
            server.battleManager.CreateBattle(dungeonCreateData, (rt) =>
            {
                token.SendResponse<Result>("Game.Dungeon.Challenge", requestId, rt);
            });
        }

        public static CreateDungeon GenDungeonCreateData(ChallengeDungeon cd)
        {
            var ret = new CreateDungeon();
            ret.dungeonTmpl = cd.dungeonTmpl;
            return ret;
        }
    }
}