using mana.Foundation;
using System;
using xxd.sync;

namespace BattleSystem.Util
{
    public static class DamageHelper
    {
        public static Damage CreatDamage(float baseValue, int damageType, int damageFlag = 0)
        {
            var damage = ObjectCache.Get<Damage>();
            damage.baseValue = (int)(baseValue + 0.5f);
            damage.value = damage.baseValue;
            damage.damageType = damageType;
            damage.damageFlag = damageFlag;
            return damage;
        }

        public static bool IsPhysicalDamage(this Damage damage)
        {
            return damage.damageType == Damage.dtype_physical;
        }

        public static bool IsMagicDamage(this Damage damage)
        {
            return damage.damageType == Damage.dtype_magic;
        }

        public static bool IsBrokenShield(this Damage damage)
        {
            throw new NotImplementedException();
        }

        public static bool IsIgnoreShield(this Damage damage)
        {
            throw new NotImplementedException();
        }


        public static void ChangeValuePercent(this Damage damage, float pv)
        {
            damage.ChangeValue((int)(damage.baseValue * pv + 0.5f));
        }

        public static void ChangeValue(this Damage damage, int v)
        {
            damage.value = Math.Max(damage.value + v, 0);
        }
    }
}
