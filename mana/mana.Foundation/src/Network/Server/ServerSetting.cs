namespace mana.Foundation.Network.Server
{
    public class ServerSetting
    {
        private static readonly ServerSetting Default = new ServerSetting()
        {
            host = "127.0.0.1",
            port = 8088,
            connBuffSize = 1024,
            connMax = 128,
            bindTimeOut = 1000 * 10,
            pingTimeOut = 3000 * 10,
            plugins = { }
        };

        public string host;

        public int port;

        public int connBuffSize;

        public int connMax;

        public int bindTimeOut;

        public int pingTimeOut;

        public string[] plugins;
    }
}
