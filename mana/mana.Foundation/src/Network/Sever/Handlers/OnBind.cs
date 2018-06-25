using System;
using System.Diagnostics;

namespace mana.Foundation
{
    [MessageBinding("Connector.Bind", ProtoType.Request, null, null, true)]
    public sealed class OnBind : IMessageHandler
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