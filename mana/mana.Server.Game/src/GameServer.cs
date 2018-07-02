using mana.Foundation.Network.Sever;
using System.Net;

namespace mana.Server.Game
{
    class GameServer : IOCPServer
    {
        public static GameServer StartNew(AppSetting setting)
        {
            var sev = new GameServer(setting);
            var ipa = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);
            //sev.Start(new IPEndPoint(ipa, setting.port));
            return sev;
        }

        public readonly BSManager battleSevsChannel;

        private GameServer(AppSetting setting) : base(setting)
        {
            battleSevsChannel = new BSManager(setting.battleServers);
            battleSevsChannel.StartConnect();
        }
    }
}
