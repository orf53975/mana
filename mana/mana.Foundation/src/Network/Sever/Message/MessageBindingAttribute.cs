using System;

namespace mana.Foundation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageBindingAttribute : Attribute
    {
        public readonly string Route;

        public readonly ProtoType ProtoType;

        public readonly Type InDataType;

        public readonly Type OutDataType;

        public readonly bool Overrideable;

        public MessageBindingAttribute(string route, ProtoType pType, Type inType, Type outType, bool overrideable = false)
        {
            this.Route = route;
            this.ProtoType = pType;
            this.InDataType = inType;
            this.OutDataType = outType;
            this.Overrideable = overrideable;
        }
    }
}