namespace mana.Server.Test.Config
{
    public class AppSetting
    {
        public string host = "127.0.0.1";

        public int port = 8088;

        public int connBuffSize = 1024;

        public int connMax = 0xFF;

        //public string[] plugins = { "Extend.Abilities.dll", "Extend.AI.dll", "Extend.Buffs.dll" };
        public string[] plugins = {};
    }
}
