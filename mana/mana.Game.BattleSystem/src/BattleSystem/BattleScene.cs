using BattleSystem.Sync;
using BattleSystem.Units;
using mana.Foundation;
using System;
using System.Collections.Generic;
using xxd.sync;
using xxd.sync.opration;

namespace BattleSystem
{
    public class BattleScene
    {
        public enum BattleType : byte { normal = 0, arena = 1 }

        public readonly BattleType battleType;

        readonly List<Unit> units = new List<Unit>();

        public readonly Random roll = new Random();

        public readonly BattleRecorder recorder = new BattleRecorder();

        public readonly long UUID;

        internal float frameTime
        {
            get;
            private set;
        }

        public bool Destroyable
        {
            get;
            private set;
        }

        public BattleScene(BattleCreateData battleCreateData)
        {
            battleType = (BattleType)battleCreateData.type;
            UUID = battleCreateData.uuid;
            foreach (var ud in battleCreateData.units)
            {
                this.AddUnit(new Unit(this, ud));
            }
        }

        public Unit Find(int id)
        {
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (units[i].uid == id)
                {
                    return units[i];
                }
            }
            return null;
        }


        public void GetUnits(List<Unit> ret, Predicate<Unit> match)
        {
            Unit u;
            for (var i = units.Count - 1; i >= 0; i--)
            {
                u = units[i];
                if (match(u))
                {
                    ret.Add(u);
                }
            }
        }

        public void ForEach(Func<Unit, bool> action)
        {
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (action(units[i]) == false)
                {
                    return;
                }
            }
        }

        public void AddUnit(Unit u)
        {
            units.Add(u);
        }

        public void AddSummonUnit(Unit u)
        {
            throw new NotImplementedException();
        }

        public void AddBullet(Unit u)
        {
            throw new NotImplementedException();
        }

        public void OnOprationRequest(CastRequest cr)
        {
            var unit = Find(cr.unitId);
            if (unit != null)
            {
                unit.OnPlayerOprationRequest(cr);
            }
        }

        public void OnOprationRequest(MoveRequest mr)
        {
            var unit = Find(mr.unitId);
            if (unit != null)
            {
                unit.OnPlayerOprationRequest(mr);
            }
        }

        private byte[] Update(float deltaTime)
        {
            this.frameTime = deltaTime;
            for (int i = units.Count - 1; i >= 0; i--)
            {
                units[i].Update();
            }
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (units[i].destroyable)
                {
                    recorder.PushRemoveUnit(units[i]);
                    units.RemoveAt(i);
                }
                else
                {
                    units[i].LateUpdate();
                }
            }
            return recorder.Build();
        }

        public void DoUpdate(float deltaTime)
        {


            //var ud = this.Update(deltaTime);
            //return new Packet(0x2001, ud);

        }

        public Packet GetBattleSnapshot()
        {
            //TODO
            // -- 1 
            //var bsd = DooObjectPool.Instance.Get<BattleSnapData>();
            //bsd.type = (byte)this.battleType;
            //bsd.uuid = this.UUID;

            //bsd.units = new UnitInfo[units.Count];
            //for (int i = units.Count - 1; i >= 0; i--)
            //{
            //    bsd.units[i] = this.units[i].GetSnapData();
            //}

            //var p = new Packet(0x2005);
            //p.Put(bsd);

            //DooObjectPool.Instance.Recycle(ref bsd);
            //return p;
            return null;
        }

    }
}