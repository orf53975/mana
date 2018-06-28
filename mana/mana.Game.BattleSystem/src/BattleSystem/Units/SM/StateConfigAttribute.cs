using System;

namespace BattleSystem.Units.SM
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class StateConfigAttribute : Attribute
    {
        public int priority
        {
            get;
            set;
        }

        public bool openAI
        {
            get;
            set;
        }

        public Unit.ActionState actionState
        {
            get;
            set;
        }
    }
}
