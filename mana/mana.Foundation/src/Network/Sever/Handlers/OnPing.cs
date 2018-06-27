namespace mana.Foundation.Network.Sever
{
    [MessageNotify("Connector.Ping", typeof(Heartbeat), true)]
    public sealed class OnPing : IMessageHandler
    {
        public void Process(UserToken token, Packet msg)
        {
            for(int i = 0; i < 1000; i ++)
            {
                token.SendPush<Heartbeat>("Connector.Pong", (hb) =>
                {
                    hb.timestamp = i;
                });
            }
            //token.SendPush<Heartbeat>("Connector.Pong", (hb) =>
            //{
            //    hb.timestamp = TimeUtil.GetCurrentTime();
            //});
        }
    }
}