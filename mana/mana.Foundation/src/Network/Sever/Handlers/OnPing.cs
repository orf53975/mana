namespace mana.Foundation
{
    [MessageBinding("Connector.Ping", ProtoType.Notify, typeof(Heartbeat), null, true)]
    public sealed class OnPing : IMessageHandler
    {
        public void Process(UserToken token, Packet msg)
        {
            token.SendPush<Heartbeat>("Connector.Pong", (hb) =>
            {
                hb.timestamp = TimeUtil.GetCurrentTime();
            });
        }
    }
}