using mana.Foundation;

namespace mana.Foundation
{
    [MessageBinding("Connector.Ping")]
    public sealed class OnPing : IMessageHandler
    {
        public void Process(UserToken token, Packet msg)
        {
            token.SendPush(msg.msgRoute, null);
        }
    }
}