using BattleSystem.Sync;
using BattleSystem.Units;
using mana.Foundation;
using System;
using System.Collections.Generic;
using xxd.battle;
using xxd.battle.opration;

namespace BattleSystem
{
    public class BattleScene
    {
        #region <<Definition IPlayerMessagePusher>>
        public interface IPlayerMessagePusher
        {
            void Push(string channelToken, Packet p);
        }
        #endregion

        public enum BattleType : byte { normal = 0, arena = 1 }

        public readonly BattleType battleType;

        readonly List<Unit> units = new List<Unit>();

        public readonly Random roll = new Random();

        public readonly BattleRecorder recorder = new BattleRecorder();

        public readonly string uid;

        public readonly IPlayerMessagePusher playerMessagePusher;

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

        public BattleScene(BattleCreateInfo bci, string requestor, IPlayerMessagePusher pmp)
        {
            this.uid = requestor + "-" + Guid.NewGuid().ToString();
            this.playerMessagePusher = pmp;
            this.InitWithTemplate(bci);
        }

        private void InitWithTemplate(BattleCreateInfo bci)
        {
            //TODO
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

        public void AddUnit(UnitCreateData ucd)
        {
            var u = new Unit(this, ucd);
            this.AddUnit(u);
        }


        public void Update(float deltaTime)
        {
            this.frameTime = deltaTime;
            for (int i = units.Count - 1; i >= 0; i--)
            {
                units[i].Update();
            }
            for (int i = units.Count - 1; i >= 0; i--)
            {
                var u = units[i];
                if (u.destroyable)
                {
                    recorder.PushRemoveUnit(u);
                    units.RemoveAt(i);
                }
                else
                {
                    u.LateUpdate();
                }
            }
            this.PushBattleSyncToAllPlayer();
        }

        private void PushBattleSyncToAllPlayer()
        {
            //token.SendPush<BattleSync>("Battle.Sync", (bs) =>
            //{
            //    bs.actions = new DataObject[3];
            //    bs.actions[0] = ObjectCache.Get<AddUnit>();
            //    bs.actions[1] = ObjectCache.Get<RemoveUnit>();
            //    bs.actions[2] = ObjectCache.Get<BuffData>();
            //}, packet.msgAttachId);
            //TODO
            //var packet = Packet.CreatPush()
            //for (int i = units.Count - 1; i >= 0; i--)
            //{
            //    var u = units[i];
            //    if (u.channelToken != null)
            //    {

            //    }
            //}
            //return recorder.Build();
        }


        //TODO
        //public void GetBattleSnapshot(BattleSnapData bsd)
        //{
        //    bsd.type = (byte)this.battleType;
        //    bsd.uuid = this.UUID;
        //    bsd.units = new UnitInfo[units.Count];
        //    for (int i = units.Count - 1; i >= 0; i--)
        //    {
        //        bsd.units[i] = this.units[i].GetSnapData();
        //    }
        //}

        #region <<Message Request>>

        public void OnOprationRequest(string playerId, CastRequest cr)
        {
            var unit = Find(cr.unitId);
            if (unit != null)
            {
                unit.OnPlayerOprationRequest(cr);
            }
        }

        public void OnOprationRequest(string playerId, MoveRequest mr)
        {
            var unit = Find(mr.unitId);
            if (unit != null)
            {
                unit.OnPlayerOprationRequest(mr);
            }
        }

        #endregion
    }
}