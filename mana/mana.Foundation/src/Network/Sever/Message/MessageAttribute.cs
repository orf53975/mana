using System;

namespace mana.Foundation.Network.Sever
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageConfigAttribute : Attribute
    {
        public readonly ProtoType protoType;

        public readonly string route;

        public readonly Type inType;

        public readonly Type outType;

        public readonly int overridePriority;

        public MessageConfigAttribute(ProtoType protoType, string route, Type inType, Type outType, int overridePriority = 1)
        {
            this.protoType = protoType;
            this.route = route;
            this.inType = inType;
            this.outType = outType;
            this.overridePriority = overridePriority;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageRequestAttribute : MessageConfigAttribute
    {
        public MessageRequestAttribute(string route, Type inType, Type outType, int overridePriority = 1)
            : base(ProtoType.Request, route, inType, outType, overridePriority)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageNotifyAttribute : MessageConfigAttribute
    {
        public MessageNotifyAttribute(string route, Type inType, int overridePriority = 1)
            : base(ProtoType.Notify, route, inType, null, overridePriority)
        {
        }
    }
}