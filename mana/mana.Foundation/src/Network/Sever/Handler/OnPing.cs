namespace mana.Foundation.Network.Sever
{
    [MessageNotify("Connector.Ping", typeof(Heartbeat), 0)]
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