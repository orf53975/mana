using BattleSystem.Lang;
using mana.Foundation;
using System;

namespace BattleSystem.Units.AI
{
    [AIConfig(tmplId = 2001)]
    public class NormalMonsterAI : AIEntity
    {
        public NormalMonsterAI(Unit unit) : base(unit)
        {
        }

        public Unit TargetEnemy
        {
            get;
            private set;
        }


        /// <summary>
        /// 普通AI
        /// </summary>
        /// <returns></returns>
        private UnitOrder SelectNormalOrder()
        {
            if (!MathTools.IsInArea(host.x, host.z, host.startX, host.startZ, 8))
            {
                return UnitOrder.Pool.GetWanderOrder(host.startX, host.startZ);
            }
            else if (host.Roll(1.0f) < 0.6f)
            {
                var ox = host.Roll(10);
                var oz = host.Roll(10);
                if (ox + oz < 3.0f)
                {
                    return null;
                }
                ox += host.x - 5.0f;
                oz += host.z - 5.0f;
                if (!MathTools.IsInArea(ox, oz, host.startX, host.startZ, 8))
                {
                    return null;
                }
                return UnitOrder.Pool.GetWanderOrder(ox, oz);
            }
            else
            {
                return UnitOrder.Pool.GetIdleOrder();
            }
        }

        /// <summary>
        /// 战斗AI
        /// </summary>
        /// <returns></returns>
        private UnitOrder SelectCombatOrder()
        {
            if (!host.IsTargetInRange(TargetEnemy, host.atkRange + TargetEnemy.bodySize))
            {
                return UnitOrder.Pool.GetMoveToTargetOrder(TargetEnemy, host.atkRange);
            }
            else
            {
                return UnitOrder.Pool.GetAbilityCastOrder(1001, TargetEnemy);
            }
        }

        public override UnitOrder SelectOrder()
        {
            if (TargetEnemy != null && (TargetEnemy.dead || !host.IsTargetInRange(TargetEnemy, host.fov)))
            {
                TargetEnemy = null;
            }
            if (TargetEnemy == null)
            {
                TargetEnemy = host.FindNearestEnemyInFov();
            }
            if (TargetEnemy != null)
            {
                return SelectCombatOrder();
            }
            if (host.IsInNullState())
            {
                return SelectNormalOrder();
            }
            return null;
        }
    }
}
