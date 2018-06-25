namespace mana.Foundation
{
    [MessageBinding("Connector.Ping", ProtoType.NOTIFY, typeof(Heartbeat), null, true)]
    public sealed class OnPing : IMessageHandler
    {
        public void Process(UserToken token, Packet msg)
        {
            token.SendPush("Connector.Pong", new Heartbeat()
            {
                timestamp = TimeUtil.GetCurrentTime()
            });
        }
    }
}