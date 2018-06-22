using System;

namespace mana.Foundation
{
    public class DataObjectConfigAttribute : Attribute
    {
        public ushort TypeCode
        {
            get;
            set;
        }
    }
}