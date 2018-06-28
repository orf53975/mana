using System;

namespace BattleSystem.Units.AI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AIConfigAttribute : Attribute
    {
        public int tmplId
        {
            get;
            set;
        }
    }
}
