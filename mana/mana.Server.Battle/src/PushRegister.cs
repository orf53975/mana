using mana.Foundation.Network.Sever;
using xxd.battle;

namespace mana.Server.Battle
{
    class PushRegister : IPushRegister
    {
        public void RegistPushMessage()
        {
            ProtocolManager.AddProtoPush<BattleSync>("Battle.Sync");
        }
    }
}
