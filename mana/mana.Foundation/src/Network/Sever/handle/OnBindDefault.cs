using mana.Foundation;
using System;
using System.Diagnostics;

namespace mana.Foundation
{
    [MessageBinding("Connector.BindToken", true)]
    public sealed class OnBindDefault : IMessageHandler
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