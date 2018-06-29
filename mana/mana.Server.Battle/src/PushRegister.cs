using mana.Foundation;
using mana.Foundation.Network.Sever;
using xxd.sync;

namespace mana.Server.Battle
{
    class PushRegister : IPushRegister
    {
        public void DoRegist()
        {
            ProtocolManager.AddProtoPush<BattleSync>("Battle.Sync");
            ////...................


        }
    }
}
