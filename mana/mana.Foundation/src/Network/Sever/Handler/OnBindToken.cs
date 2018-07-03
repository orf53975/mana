using System;

namespace mana.Foundation.Network.Sever
{
    [MessageRequest("Connector.BindToken", typeof(AccountInfo), typeof(Response), 0)]
    public sealed class OnBindToken : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            var error = token.TryBind(Guid.NewGuid().ToString());
            if (error != null)
            {
                Logger.Error(error);
            }
        }
    }
}