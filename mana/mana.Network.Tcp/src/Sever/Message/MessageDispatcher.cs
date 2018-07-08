using System;
using System.Collections.Generic;

namespace mana.Foundation.Network.Sever
{
    public class MessageDispatcher
    {
        public readonly static MessageDispatcher Instance = new MessageDispatcher();

        readonly Dictionary<string, IMessageHandler> handlers;

        private MessageDispatcher()
        {
            handlers = new Dictionary<string, IMessageHandler>();
        }

        public bool RegistHandler(Type msgHandlerType)
        {
            var mca = msgHandlerType.TryGetAttribute<MessageConfigAttribute>();
            if (mca == null)
            {
                Logger.Error("Regist falied! {0} require MessageConfigAttribute", msgHandlerType.FullName);
                return false;
            }
            IMessageHandler existed;
            if (handlers.TryGetValue(mca.route, out existed))
            {
                var existed_mca = existed.GetType().TryGetAttribute<MessageConfigAttribute>();
                if (existed_mca.overridePriority == mca.overridePriority)
                {
                    Logger.Error("MessageHandler conflict! {0}->{1}", existed.GetType(), msgHandlerType);
                    return false;
                }
                if (existed_mca.overridePriority > mca.overridePriority)
                {
                    return false;
                }
            }
            handlers[mca.route] = Activator.CreateInstance(msgHandlerType) as IMessageHandler;
            if (mca.genProto)
            {
                ProtocolManager.AddProto(mca.route, mca.protoType, mca.inType, mca.outType);
            }
            return true;
        }

        public bool Dispatch(UserToken token, Packet p)
        {
            IMessageHandler handler;
            if (handlers.TryGetValue(p.msgRoute, out handler))
            {
                handler.Process(token, p);
                return true;
            }
            return false;
        }
    }
}
