using System;

namespace BattleSystem.Units.Abilities
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AbilityConfigAttribute : Attribute
    {

        public string name
        {
            get;
            set;
        }

        public int tmplId
        {
            get;
            set;
        }

        public AbilityFlag flag
        {
            get;
            set;
        }


        public bool testFlag(AbilityFlag _flag)
        {
            return (flag & _flag) == _flag;
        }
    }
}