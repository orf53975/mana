using mana.Foundation.Network.Sever;

namespace mana.Server.Game
{
    class AppSetting : ServerSetting
    {
        public string name = "测试服1";

        public string[] battleServers = {
            "127.0.0.1:8082"
        };
    }
}
