using System;
using System.Diagnostics;

namespace mana.Foundation.Network.Sever
{
    [MessageRequest("Connector.BindToken", typeof(Heartbeat), typeof(Heartbeat), true)]
    public sealed class OnBindToken : IMessageHandler
    {
        public void Process(UserToken token, Packet msg)
        {
            var error = token.TryBind(Guid.NewGuid().ToString());
            if (error != null)
            {
                Trace.TraceError(error);
            }
        }
    }
}