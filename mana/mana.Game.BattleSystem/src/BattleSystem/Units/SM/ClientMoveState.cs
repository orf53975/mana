using xxd.battle.opration;

namespace BattleSystem.Units.SM
{
    [StateConfig(priority = 0, actionState = Unit.ActionState.move, openAI = true)]
    class ClientMoveState : State
    {
        private float duration;
   
        public ClientMoveState(Unit unit) : base(unit) {}

        public void DelayStop(float delay = 0.6f)
        {
            this.duration = passedTime + delay;
        }

        public override void OnPlayerOperation(MoveRequest mr)
        {
            host.SetPosition(mr.x, mr.y, mr.z);
            host.SetFaceTo(mr.faceTo);
            if (mr.type == MoveRequest.type_end)
            {
                host.TryChangeState<IdleState>();
            }
            else
            {
                this.DelayStop();
            }
        }

        public override void OnPlayerOperation(CastRequest cr)
        {
            host.TryChangeState<AbilityCast>(st =>
            {
                st.SetAbility(host.TryGetAbility(cr.abilityId));
                st.SetTarget(host.battle.Find(cr.targetId));
            });
        }

        public override bool TestEnter()
        {
            return !host.dead;
        }

        protected override void OnEnter()
        {
            var anim = host.GetDefaultAnimation(Unit.ActionState.move);
            host.PlayAnimation(anim);
        }

        protected override bool OnUpdate()
        {
            return duration < 0 || passedTime < duration;
        }

        protected override void OnExit()
        {
            this.duration = 0;
        }
    }
}
