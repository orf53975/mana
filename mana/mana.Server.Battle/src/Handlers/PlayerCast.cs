using mana.Foundation;
using mana.Foundation.Network.Server;
using xxd.battle.opration;

namespace mana.Server.Battle.src.Handlers
{
    [MessageNotify("Battle.PlayerCast", typeof(CastRequest))]
    class PlayerCast : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            var playerId = packet.msgAttach;
            var scn = token.GetServer<BattleServer>().FindBattle(token.uid, playerId);
            if (scn != null)
            {
                var req = packet.TryGet<CastRequest>();
                scn.OnOprationRequest(playerId, req);
                req.ReleaseToCache();
            }
            else
            {
                Logger.Error("can't find battleScene!");
            }
        }
    }
}
