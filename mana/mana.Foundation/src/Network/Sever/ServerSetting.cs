namespace mana.Foundation.Network.Sever
{
    public class ServerSetting
    {
        private static readonly ServerSetting Default = new ServerSetting();

        public string host = "127.0.0.1";

        public int port = 8088;

        public int connBuffSize = 1024;

        /// <summary>
        /// 最大链接数
        /// </summary>
        public int connMax = 0xFF;

        public int tokenUnbindTimeOut = 1000 * 10;

        public int tokenWorkTimeOut = 3000 * 10;

        public string[] plugins = { };

        public void UpdateWith(ServerSetting other)
        {
            if (other.host != Default.host)
            {
                this.host = other.host;
            }
            if (other.port != Default.port)
            {
                this.port = other.port;
            }
            if (other.connBuffSize != Default.connBuffSize)
            {
                this.connBuffSize = other.connBuffSize;
            }
            if (other.connMax != Default.connMax)
            {
                this.connMax = other.connMax;
            }
            if (other.tokenUnbindTimeOut != Default.tokenUnbindTimeOut)
            {
                this.tokenUnbindTimeOut = other.tokenUnbindTimeOut;
            }
            if (other.tokenWorkTimeOut != Default.tokenWorkTimeOut)
            {
                this.tokenWorkTimeOut = other.tokenWorkTimeOut;
            }
            if (other.plugins != Default.plugins)
            {
                this.plugins = other.plugins;
            }
        }
    }
}
