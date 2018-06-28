using System;
using mana.Foundation;

namespace BattleSystem.Units.Abilities
{
    public abstract class Ability
    {
        public readonly AbilityConfigAttribute configAttr;

        public readonly string name;

        public readonly int tmplId;

        public readonly bool hasPropertyBonus;

        public readonly bool isActiveUse;

        public readonly Unit host;

        public Ability(Unit unit)
        {
            this.configAttr = this.GetType().TryGetAttribute<AbilityConfigAttribute>();
            if (configAttr != null)
            {
                name = configAttr.name;
                tmplId = configAttr.tmplId;
                hasPropertyBonus = configAttr.testFlag(AbilityFlag.PropertyBonus);
                isActiveUse = configAttr.testFlag(AbilityFlag.ActiveUse);
            }
            else
            {
                name = null;
                tmplId = 0;
                hasPropertyBonus = false;
                isActiveUse = false;
            }
            this.host = unit;
        }


        #region <<Level>>

        private int _level = 0;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    OnLevelChange();
                }
            }
        }

        protected virtual void OnLevelChange()
        {
        }

        #endregion


        public virtual PropertyInfo PropertyBonus()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 测试释放技能 如果返回true 才可以释放技能
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool TestCastTo(Unit target)
        {
            if (!isActiveUse)
            {
                return false;
            }
            if (host.silence && !configAttr.testFlag(AbilityFlag.IgnoreSilence))
            {
                return false;
            }
            if (host.stun && !configAttr.testFlag(AbilityFlag.IgnoreStun))
            {
                return false;
            }
            return true;
        }


        protected Unit target = null;
        /// <summary>
        /// 向目标释放
        /// </summary>
        /// <param name="target"></param>
        public void CastTo(Unit target)
        {
            this.target = target;
            this.InitCast();
        }

        /// <summary>
        /// 释放前初始化状态
        /// </summary>
        protected virtual void InitCast()
        {
            throw new NotImplementedException(this.GetType().Name);
        }

        /// <summary>
        /// 释放逻辑
        /// </summary>
        /// <returns></returns>
        public virtual bool DoCasting()
        {
            throw new NotImplementedException(this.GetType().Name);
        }

    }
}