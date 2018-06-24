using System;

namespace mana.Foundation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageBindingAttribute : Attribute
    {
        public string route { get; set; }

        public bool overrideable { get; set; }

        public MessageBindingAttribute(string route , bool overrideable = false)
        {
            this.route = route;
            this.overrideable = overrideable;
        }
    }
}