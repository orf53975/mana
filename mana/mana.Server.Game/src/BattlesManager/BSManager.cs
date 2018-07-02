using mana.Foundation;
using mana.Foundation.Network.Client;
using System.Threading;

namespace mana.Server.Game
{
    class BSManager
    {
        private readonly BSChannel[] battleChannels;

        public BSManager(string[] battleServerAddrs)
        {
            var num = battleServerAddrs != null ? battleServerAddrs.Length : 0;
            battleChannels = new BSChannel[num];
            for (int i = 0; i < num; i++)
            {
                battleChannels[i] = new BSChannel(battleServerAddrs[i]);
            }
        }


        internal void StartConnect()
        {
            for (int i = 0; i < battleChannels.Length; i++)
            {
                battleChannels[i].StartConnect();
            }
        }
    }
}
