using System;

namespace mana.Foundation.Network.Sever
{
    public abstract class MessageConfigAttribute : Attribute
    {
        public readonly ProtoType protoType;

        public readonly string route;

        public readonly Type inType;

        public readonly Type outType;

        public readonly bool overrideable;

        protected MessageConfigAttribute(ProtoType protoType, string route, Type inType, Type outType, bool overrideable = false)
        {
            this.protoType = protoType;
            this.route = route;
            this.inType = inType;
            this.outType = outType;
            this.overrideable = overrideable;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageNotifyAttribute : MessageConfigAttribute
    {
        public MessageNotifyAttribute(string route, Type inType, bool overrideable = false)
            : base(ProtoType.Notify, route, inType, null, overrideable)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageRequestAttribute : MessageConfigAttribute
    {
        public MessageRequestAttribute(string route, Type inType, Type outType, bool overrideable = false)
            : base(ProtoType.Request, route, inType, outType, overrideable)
        {
        }
    }
}