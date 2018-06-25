using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace mana.Foundation
{
    public class MessageDispatcher
    {
        public readonly static MessageDispatcher Instance = new MessageDispatcher();

        readonly Dictionary<string, IMessageHandler> handlers;

        private MessageDispatcher()
        {
            handlers = new Dictionary<string, IMessageHandler>();
            RegistAllMessageHandler();
        }

        private void RegistAllMessageHandler()
        {
            var mhts = AppDomain.CurrentDomain.FindAllTypes((type) =>
            {
                if (type.IsClass && !type.IsAbstract && typeof(IMessageHandler).IsAssignableFrom(type))
                {
                    return true;
                }
                return false;
            });

            var overrideableHandlers = new Dictionary<string, IMessageHandler>();
            IMessageHandler obj = null;
            foreach (var t in mhts)
            {
                var mba = t.TryGetAttribute<MessageBindingAttribute>();
                if (mba == null)
                {
                    Trace.TraceError("MessageHandler Regist falied! {0} require MessageBindingAttribute", t.ToString());
                    continue;
                }
                if (!mba.Overrideable)
                {
                    if (handlers.TryGetValue(mba.Route, out obj))
                    {
                        Trace.TraceError("MessageHandler Regist falied! opcode conflict {0}->{1}", obj.GetType(), t);
                    }
                    else
                    {
                        handlers.Add(mba.Route, Activator.CreateInstance(t) as IMessageHandler);
                    }
                }
                else
                {
                    if (overrideableHandlers.TryGetValue(mba.Route, out obj))
                    {
                        Trace.TraceError("MessageHandler Regist falied! opcode conflict {0}->{1}", obj.GetType(), t);
                    }
                    else
                    {
                        overrideableHandlers.Add(mba.Route, Activator.CreateInstance(t) as IMessageHandler);
                    }
                }
            }
            foreach (var kv in overrideableHandlers)
            {
                if (!handlers.ContainsKey(kv.Key))
                {
                    handlers.Add(kv.Key, kv.Value);
                }
            }
        }

        public void ApplyProtocol(Protocol protocol)
        {
            foreach (var kv in handlers)
            {
                var route = kv.Key;
                var mba = kv.Value.GetType().TryGetAttribute<MessageBindingAttribute>();
                protocol.AddProtoAutoGenCode(route, mba.ProtoType, mba.InType, mba.OutType);
            }
            Trace.TraceInformation(protocol.ToFormatString(""));
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
