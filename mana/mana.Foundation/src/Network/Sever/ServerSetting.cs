namespace mana.Foundation.Network.Sever
{
    public class ServerSetting
    {

        public string host = "127.0.0.1";

        public int port = 8088;

        /// <summary>
        /// 链接缓存大小
        /// </summary>
        public int connBuffSize = 1024;

        /// <summary>
        /// 最大链接数
        /// </summary>
        public int connMax = 0xFF;

        public int tokenUnbindTimeOut = 1000 * 10;

        public int tokenWorkTimeOut = 3000 * 10;

        public string[] plugins = { };
    }
}
