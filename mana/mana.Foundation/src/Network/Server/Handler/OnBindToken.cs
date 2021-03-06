﻿using System;

namespace mana.Foundation.Network.Server
{
    [MessageRequest("Connector.BindToken", typeof(AccountInfo), typeof(Result), 0)]
    public sealed class OnBindToken : IMessageHandler
    {
        public void Process(UserToken token, Packet packet)
        {
            var server = token.server;
            // -- 1 gen  uid
            var accountInfo = packet.TryGet<AccountInfo>();
            var uId = server.GenUID(accountInfo);
            accountInfo.ReleaseToCache();
            // -- 2 kick old 
            server.TryKick(uId);
            // -- 3 bind uid
            var error = token.TryBind(uId);
            if (error == null)
            {
                token.SendResponse<Result>("Connector.BindToken", packet.msgRequestId,
                    (response) =>
                    {
                        response.code = Result.Code.sucess;
                    });
            }
            else
            {
                Logger.Error(error);
            }
        }
    }
}