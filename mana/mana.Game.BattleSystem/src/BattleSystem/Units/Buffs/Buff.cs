using System;
using mana.Foundation;

namespace BattleSystem.Units.Buffs
{
    public abstract class Buff
    {
        public readonly BuffConfigAttribute configAttr;

        public readonly string name;

        public readonly int tmplId;

        public readonly bool hasPropertyBonus;

        public readonly bool dispellable;

        public readonly bool stackable;

        public readonly int lev;

        public Buff(int level)
        {
            this.configAttr = this.GetType().TryGetAttribute<BuffConfigAttribute>();
            if (configAttr != null)
            {
                this.name = configAttr.name;
                this.tmplId = configAttr.tmplId;
                this.hasPropertyBonus = configAttr.testFlag(BuffFlag.PropertyBonus);
                this.dispellable = !configAttr.testFlag(BuffFlag.NoDispellable);
                this.stackable = configAttr.testFlag(BuffFlag.Stackable);
            }
            else
            {
                this.name = this.GetType().Name;
                this.tmplId = 0;
                this.hasPropertyBonus = false;
                this.dispellable = true;
                this.stackable = false;
            }
            this.lev = level;
        }

        protected Unit host
        {
            get;
            private set;
        }

        /// <summary>
        /// 叠加层数
        /// </summary>
        public int stacks
        {
            get;
            protected set;
        }

        public string OnAddedTo(Unit unit)
        {
            if (this.host != null)
            {
                return "buff->onAddedTo error!!!";
            }
            this.host = unit;
            this.Apply();
            return null;
        }


        public string OnRemovedFromHost()
        {
            if (this.host == null)
            {
                return "buff->onRemovedFromHost error!!!";
            }
            this.host = null;
            return null;
        }

        public virtual void Apply()
        {
        }

        /// <summary>
        /// 叠加操作
        /// </summary>
        public virtual void DoStackingUp()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 是否排除指定BUFF
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public virtual bool Exclude(Buff other)
        {
            return false;
        }

        public virtual PropertyInfo PropertyBonus()
        {
            throw new NotImplementedException();
        }
    }
}
