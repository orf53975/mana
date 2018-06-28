using BattleSystem.Units.Abilities;
using BattleSystem.Units.Events;
using System;
using System.Collections.Generic;
using System.Text;
using mana.Foundation;
using xxd.sync;

namespace BattleSystem.Units
{
    public partial class Unit
    {
        private readonly List<Ability> abilities = new List<Ability>();

        public Ability TryGetAbility(int tmplId)
        {
            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                if (abilities[i].tmplId == tmplId)
                {
                    return abilities[i];
                }
            }
            return null;
        }

        public Ability TryGetAbility<T>() where T : Ability
        {
            var ac = typeof(T).TryGetAttribute<AbilityConfigAttribute>();
            if (ac == null)
            {
                return null;
            }
            return TryGetAbility(ac.tmplId);
        }

        public string TryAddAbility(Ability ab, bool isRefreshProperty = true)
        {
            if (TryGetAbility(ab.tmplId) != null)
            {
                return "ability already exist:" + ab.tmplId;
            }
            this.abilities.Add(ab);
            if (ab is IUnitEventHandler)
            {
                this.AddEventHandler(ab as IUnitEventHandler);
            }
            if (isRefreshProperty && ab.hasPropertyBonus)
            {
                this.InitProperty();
            }
            return null;
        }

        public string TryAddAbility<T>(int lev, bool isRefreshProperty = true) where T : Ability
        {
            var ab = AbilityFactory.Creat<T>(this, lev);
            if (ab == null)
            {
                return "ability has not been registered:" + typeof(T).Name;
            }
            return TryAddAbility(ab, isRefreshProperty);
        }

        public string TryAddAbility(int tmplId, int lev, bool isRefreshProperty = true)
        {
            var ab = AbilityFactory.Creat(this, tmplId, lev);
            if (ab == null)
            {
                return "ability has not been registered:" + tmplId;
            }
            return TryAddAbility(ab, isRefreshProperty);
        }

        public string TryRemoveAbility<T>(bool isRefreshProperty = true) where T : Ability
        {
            var ac = typeof(T).TryGetAttribute<AbilityConfigAttribute>();
            if (ac == null)
            {
                return "ability has not been registered:" + typeof(T).Name;
            }
            return TryRemoveAbility(ac.tmplId, isRefreshProperty);
        }

        public string TryRemoveAbility(int tmplId, bool isRefreshProperty = true)
        {
            var ab = this.TryGetAbility(tmplId);
            if (ab == null)
            {
                return "rmvAbility -> can't find ability:" + tmplId;
            }
            if(!abilities.Remove(ab))
            {
                return "rmvAbility -> failed!";
            }
            if (ab is IUnitEventHandler)
            {
                this.RmvEventHandler(ab as IUnitEventHandler);
            }
            if (isRefreshProperty && ab.hasPropertyBonus)
            {
                this.InitProperty();
            }
            return null;
        }

        public AbilityData GetAbilityData(int tmplId)
        {
            for (int i = baseProp.abilitiesData.Length - 1; i >= 0; i--)
            {
                if(baseProp.abilitiesData[i].tmplId == tmplId)
                {
                    return baseProp.abilitiesData[i];
                }
            }
            return null;
        }

        private void InitAbilities()
        {
            if (baseProp.abilitiesData != null)
            {
                var errorInfo = new StringBuilder();
                foreach (var ad in baseProp.abilitiesData)
                {
                    var err = this.TryAddAbility(ad.tmplId, ad.lev, false);
                    if (err != null)
                    {
                        errorInfo.AppendLine(err);
                    }
                }
                if (errorInfo.Length > 0)
                {
                    throw new Exception(errorInfo.ToString());
                }
            }
        }

        private PropertyInfo CalAbilitiesPropertyBonus()
        {
            var ret = new PropertyInfo();
            foreach (var ab in abilities)
            {
                if (ab.hasPropertyBonus)
                {
                    ret += ab.PropertyBonus();
                }
            }
            return ret;
        }
    }
}