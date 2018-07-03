using mana.Foundation;
using mana.Foundation.Network.Sever;
using mana.Server.Game.BattleLink;
using System.Net;

namespace mana.Server.Game
{
    class GameServer : IOCPServer
    {
        public static GameServer StartNew(AppSetting setting)
        {
            var sev = new GameServer(setting);
            var ipa = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);
            sev.Start(new IPEndPoint(ipa, setting.port));
            sev.battleManager.StartConnect();
            return sev;
        }

        private readonly BSCMgr battleManager;

        private GameServer(AppSetting setting) : base(setting)
        {
            battleManager = new BSCMgr(setting.battleServers);
        }

        /// <summary>
        /// 转发消息至战斗服
        /// </summary>
        /// <param name="gamePlayer"></param>
        /// <param name="packet"></param>
        internal void ForwardingToBattleServer(GamePlayer gamePlayer, Packet packet)
        {
            packet.SetToken(gamePlayer.uid);
            battleManager.Send(gamePlayer.bscId, packet);
        }

    }
}
