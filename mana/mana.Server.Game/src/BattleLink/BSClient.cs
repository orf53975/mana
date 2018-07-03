using mana.Foundation;
using mana.Foundation.Network.Client;
using System.Net;

namespace mana.Server.Game.BattleLink
{
    class BSClient
    {
        private readonly IPEndPoint Addr;

        internal NetClient Channel
        {
            get;
            private set;
        }

        public BSClient(string addr)
        {
            this.Addr = Utils.GetIPEndPoint(addr);
        }

        internal void StartConnect()
        {
            if (Channel == null)
            {
                Channel = new NetClientIOCP(20 * 1000, 2048, 1024, true);
            }
            Channel.Connect(Addr, (sucess) =>
            {

            });
        }

    }
}
