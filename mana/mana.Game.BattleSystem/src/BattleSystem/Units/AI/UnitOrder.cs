

using mana.Foundation;

namespace BattleSystem.Units.AI
{
    public class UnitOrder
    {
        public enum OrderType : byte
        {
            idle = 0,           //休息
            moveToTarget = 1,   //向目标前进 
            castAbility = 2,    //向目标释放技能
            wander = 3,         //游荡
            patrol = 4          //巡逻
        }

        public OrderType orderType;


        public float idleTime
        {
            get;
            private set;
        }

        public int abilityId
        {
            get;
            private set;
        }

        public Unit target
        {
            get;
            private set;
        }

        public float range
        {
            get;
            private set;
        }

        public float wanderX
        {
            get;
            private set;
        }

        public float wanderZ
        {
            get;
            private set;
        }

        private UnitOrder() { }

        private void Reset()
        {
            idleTime = 0f;
            abilityId = 0;
            target = null;
            range = 0f;
            wanderX = 0f;
            wanderZ = 0f;
        }


        #region <<pool>>

        internal static class Pool
        {
            private static readonly ObjectPool<UnitOrder> pool = new ObjectPool<UnitOrder>(() => new UnitOrder());

            internal static UnitOrder GetIdleOrder(float idleTime = 2.0f)
            {
                var ret = pool.Get();
                ret.orderType = OrderType.idle;
                ret.idleTime = idleTime;
                return ret;
            }

            public static UnitOrder GetAbilityCastOrder(int abilityId, Unit target)
            {
                var ret = pool.Get();
                ret.orderType = OrderType.castAbility;
                ret.abilityId = abilityId;
                ret.target = target;
                return ret;
            }

            public static UnitOrder GetMoveToTargetOrder(Unit target, float range)
            {
                var ret = pool.Get();
                ret.orderType = OrderType.moveToTarget;
                ret.range = range;
                ret.target = target;
                return ret;
            }

            internal static UnitOrder GetWanderOrder(float targetX , float targetZ)
            {
                var ret = pool.Get();
                ret.orderType = OrderType.wander;
                ret.wanderX = targetX;
                ret.wanderZ = targetZ;
                return ret;
            }

            internal static void Release(UnitOrder obj)
            {
                obj.Reset();
                pool.Put(obj);
            }

            internal static string stateInfo()
            {
                return pool.ToString();
            }
        }

        #endregion

    }
}
