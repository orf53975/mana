using mana.Foundation.Network.Server;
using xxd.battle;

namespace mana.Server.Battle
{
    class PushRegister : IPushProtoRegister
    {
        public void RegistPushProto()
        {
            ProtocolManager.AddProtoPush<BattleSync>("Battle.Sync");
        }
    }
}
