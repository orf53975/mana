using BattleSystem.Units;
using mana.Foundation;
using System;
using System.Collections.Generic;
using xxd.battle;

namespace BattleSystem.Sync
{
    public sealed class BattleRecorder
    {
        readonly List<DataObject> records = new List<DataObject>(1024);

        public BattleRecorder()
        {
        }
   
        private UnitSync GetCreateUnitSync(int unitId)
        {
            if (records.Count > 0)
            {
                UnitSync us;
                for (int i = records.Count - 1; i >= 0; i--)
                {
                    us = records[i] as UnitSync;
                    if (us != null && unitId == us.unitId)
                    {
                        return us;
                    }
                }
            }
            var ret = ObjectCache.Get<UnitSync>();
            ret.unitId = unitId;
            records.Add(ret);
            return ret;
        }

        public void PushEffectShow(Unit unit)
        {
            var us = GetCreateUnitSync(unit.uid);
            us.effectShow = (int)unit.EffectShow;
        }

        public void PushActionState(Unit unit)
        {
            var us = GetCreateUnitSync(unit.uid);
            us.actionState = (byte)unit.curActionState;
        }

        public void PushAnimPlay(Unit unit, float time)
        {
            var us = GetCreateUnitSync(unit.uid);
            us.AnimPlayCode = unit.curAnimation;
            us.AnimPlayTime = time;
        }

        public void PushFaceTo(Unit unit)
        {
            var us = GetCreateUnitSync(unit.uid);
            us.faceTo = unit.faceTo;
        }

        public void PushPosition(Unit unit, byte flag)
        {
            var us = GetCreateUnitSync(unit.uid);
            if (BitFlag.TestByteFlag(flag, 0))
            {
                us.x = unit.x;
            }
            if (BitFlag.TestByteFlag(flag, 1))
            {
                us.y = unit.y;
            }
            if (BitFlag.TestByteFlag(flag, 2))
            {
                us.z = unit.z;
            }
        }

        public void PushHpChange(Unit unit)
        {
            var us = GetCreateUnitSync(unit.uid);
            us.hpPercent = unit.hpPercent;
        }

        public void PushMpChange(Unit unit)
        {
            var us = GetCreateUnitSync(unit.uid);
            us.mpPercent = unit.mpPercent;
        }

        public void PushDamage(Damage dmg)
        {
            records.Add(dmg);
        }

        public void PushHealing(Healing hel)
        {
            records.Add(hel);
        }

        public void PushAddUnit(Unit unit)
        {
            throw new NotImplementedException();
        }

        public void PushRemoveUnit(Unit unit)
        {
            var ru = ObjectCache.Get<RemoveUnit>();
            ru.unitId = unit.uid;
            records.Add(ru);
        }

        public DataObject[] Build()
        {
            var ret = records.ToArray();
            records.Clear();
            return ret;
        }
    }
}