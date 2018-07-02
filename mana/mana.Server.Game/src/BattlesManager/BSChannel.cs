using mana.Foundation;
using mana.Foundation.Network.Client;

namespace mana.Server.Game
{
    class BSChannel
    {
        private readonly NetClient Channel;

        private readonly string Addr;

        public BSChannel(string addr)
        {
            this.Channel = new NetClientIOCP(20 * 1000, 2048, 1024, true);
            this.Addr = addr;
        }

        internal void StartConnect()
        {
            Channel.Connect(Utils.GetIPEndPoint(Addr), (sucess) =>
            {
                Logger.Print("connected {0}!", sucess ? "sucessed" : "failed");
            });
        }
    }
}
