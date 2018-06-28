using BattleSystem.Units.AI;
using BattleSystem.Units.SM;
using System;

namespace BattleSystem.Units
{
    public partial class Unit
    {
        private AIEntity mAiEntity = null;

        private void InitAiTree()
        {
            mAiEntity = AIFactory.Creat(this.baseProp.aiTmplId, this);
            if (mAiEntity == null)
            {
                mAiEntity = new ComAI(this);
            }
        }

        private void DoAI()
        {
            var uo = mAiEntity.SelectOrder();
            if (uo == null)
            {
                return;
            }
            switch (uo.orderType)
            {
                case UnitOrder.OrderType.idle:
                    this.TryChangeState<IdleState>(st =>
                    {
                        st.SetDurationOnce(uo.idleTime);
                    });
                    break;
                case UnitOrder.OrderType.moveToTarget:
                    this.TryChangeState<MoveToTargetState>(st =>
                    {
                        st.SetTarget(uo.target);
                        st.SetRange(uo.range);
                    });
                    break;
                case UnitOrder.OrderType.castAbility:
                    this.TryChangeState<AbilityCast>(st =>
                    {
                        st.SetAbility(this.TryGetAbility(uo.abilityId));
                        st.SetTarget(uo.target);
                    });
                    break;
                case UnitOrder.OrderType.wander:
                    this.TryChangeState<WanderState>(st =>
                    {
                        st.SetTarget(uo.wanderX, uo.wanderZ);
                    });
                    break;
                default:
                    throw new NotImplementedException(uo.orderType.ToString());
            }
            UnitOrder.Pool.Release(uo);
        }
    }
}