namespace mana.Server.Battle.Config
{
    /// <summary>
    /// eg:
    ///     plugins = { "Extend.Abilities.dll", "Extend.AI.dll", "Extend.Buffs.dll" };
    /// </summary>
    public class AppSetting
    {
        public string host = "127.0.0.1";

        public int port = 8088;

        public int connBuffSize = 1024;

        public int connMax = 0xFF;
        
        public string[] plugins = {};
    }
}
