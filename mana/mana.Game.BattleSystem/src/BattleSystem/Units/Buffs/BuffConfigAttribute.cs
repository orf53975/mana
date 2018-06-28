using System;

namespace BattleSystem.Units.Buffs
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class BuffConfigAttribute : Attribute
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


        public BuffFlag flag
        {
            get;
            set;
        }


        public bool testFlag(BuffFlag _flag)
        {
            return (flag & _flag) == _flag;
        }
    }
}
