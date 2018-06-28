using BattleSystem.Units.Buffs;
using BattleSystem.Units.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSystem.Units
{
    public partial class Unit
    {
        private readonly List<Buff> buffs = new List<Buff>();

        public string TryAddBuff(Buff buff, bool isRefreshProperty = true)
        {
            // -- 1 do stackingUp
            if (buff.stackable == true)
            {
                var stackTargetBuff = buffs.Find((element) =>
                {
                    return buff.tmplId == element.tmplId && buff.lev == element.lev;
                });
                if (stackTargetBuff != null)
                {
                    stackTargetBuff.DoStackingUp();
                    return null;
                }
            }
            // -- 2 exclude me
            var excludeMe = buffs.Find((element) =>
            {
                return element.Exclude(buff);
            });
            if (excludeMe != null)
            {
                return "add buff[" + buff.tmplId + "]failed: exclude by" + excludeMe.tmplId;
            }
            // -- 3 exclude others
            this.TryRemoveBuff((element) =>
            {
                return buff.Exclude(element);
            }, null, false);
            // -- 4 add buff
            var err = buff.OnAddedTo(this);
            if (err != null)
            {
                return err;
            }
            this.buffs.Add(buff);
            if (buff is IUnitEventHandler)
            {
                this.AddEventHandler(buff as IUnitEventHandler);
            }
            if (isRefreshProperty && buff.hasPropertyBonus)
            {
                this.InitProperty();
            }
            return null;
        }

        public string TryAddBuff<T>(int lev, bool isRefreshProperty = true) where T : Buff
        {
            var buff = BuffFactory.Creat<T>(lev);
            if (buff == null)
            {
                return "buff has not been registered:" + typeof(T).Name;
            }
            return TryAddBuff(buff, isRefreshProperty);
        }

        public string TryAddBuff(int tmplId, int lev, bool isRefreshProperty = true)
        {
            var buff = BuffFactory.Creat(tmplId, lev);
            if (buff == null)
            {
                return "buff has not been registered:" + tmplId;
            }
            return TryAddBuff(buff, isRefreshProperty);
        }

        public string TryRemoveBuff(Buff buff, bool isRefreshProperty = true)
        {
            var buffIndex = buffs.IndexOf(buff);
            if (buffIndex == -1)
            {
                return "can't find buff:" + buff.tmplId;
            }
            return TryRemoveBuffByIndex(buffIndex, isRefreshProperty);
        }

        private string TryRemoveBuffByIndex(int buffIndex, bool isRefreshProperty = true)
        {
            var buff = buffs[buffIndex];

            var err = buff.OnRemovedFromHost();
            if (err != null)
            {
                return err;
            }

            buffs.RemoveAt(buffIndex);
            if (buff is IUnitEventHandler)
            {
                this.RmvEventHandler(buff as IUnitEventHandler);
            }
            if (isRefreshProperty && buff.hasPropertyBonus)
            {
                this.InitProperty();
            }
            return null;
        }

        public void TryRemoveBuff(Predicate<Buff> match, List<Buff> removeds = null, bool isRefreshProperty = true)
        {
            var errInfo = new StringBuilder();
            var hadRmvPropertyBonus = false;
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                if (buffs[i].dispellable && match(buffs[i]))
                {
                    hadRmvPropertyBonus |= buffs[i].hasPropertyBonus;
                    var err = TryRemoveBuffByIndex(i, false);
                    if (err != null)
                    {
                        errInfo.AppendLine(err);
                    }
                    else if (removeds != null)
                    {
                        removeds.Add(buffs[i]);
                    }
                }
            }
            if (isRefreshProperty && hadRmvPropertyBonus)
            {
                this.InitProperty();
            }
            if (errInfo.Length > 0)
            {
                throw new Exception(errInfo.ToString());
            }
        }

        private void InitBuffs()
        {
            if(baseInfo.buffsData != null)
            {
                var errInfo = new StringBuilder();
                foreach (var bd in baseInfo.buffsData)
                {
                    var err = this.TryAddBuff(bd.tmplId, bd.lev, false);
                    if (err != null)
                    {
                        errInfo.AppendLine(err);
                    }
                }
                if (errInfo.Length > 0)
                {
                    throw new Exception(errInfo.ToString());
                }
            }
        }

        private PropertyInfo CalBuffsPropertyBonus()
        {
            var ret = new PropertyInfo();
            foreach (var buf in buffs)
            {
                if (buf.hasPropertyBonus)
                {
                    ret += buf.PropertyBonus();
                }
            }
            return ret;
        }
    }
}