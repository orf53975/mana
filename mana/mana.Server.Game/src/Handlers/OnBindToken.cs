using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;

namespace mana.Server.Game.Handler
{
    [MessageRequest("Connector.BindToken", typeof(AccountInfo), typeof(Response))]
    public sealed class OnBindToken : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            var error = token.TryBind(Guid.NewGuid().ToString());
            if (error != null)
            {
                Logger.Error(error);
            }

            token.SendResponse<Response>("Connector.BindToken", packet.msgRequestId,
                (response) =>
                {
                    response.code = Response.Code.sucess;
                });
        }
    }
}