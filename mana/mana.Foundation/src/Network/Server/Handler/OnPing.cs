namespace mana.Foundation.Network.Server
{
    [MessageNotify("Connector.Ping", typeof(Heartbeat), 0)]
    public sealed class OnPing : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            token.SendPush<Heartbeat>("Connector.Pong", (hb) =>
            {
                hb.timestamp = TimeUtil.GetCurrentTime();
            });
        }
    }
}