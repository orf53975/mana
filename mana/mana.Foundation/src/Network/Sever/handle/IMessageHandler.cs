using mana.Foundation;

namespace mana.Foundation
{
    public interface IMessageHandler
    {
        void Process(UserToken token, Packet msg);
    }
}
