using xxd.sync.opration;

namespace BattleSystem.Units.SM
{
    [StateConfig(priority = 0, openAI = true, actionState = Unit.ActionState.idle)]
    class IdleState : State
    {
        public const float kDefaultDuration = 2.0f;

        private int playAnimationCode = 0;

        private float duration = kDefaultDuration;

        public IdleState(Unit unit) : base(unit) { }

        public override void OnPlayerOperation(CastRequest cr)
        {
            host.TryChangeState<AbilityCast>(st =>
            {
                st.SetAbility(host.TryGetAbility(cr.abilityId));
                st.SetTarget(host.battle.Find(cr.targetId));
            });
        }

        public override void OnPlayerOperation(MoveRequest mr)
        {
            host.SetPosition(mr.x, mr.y, mr.z);
            host.SetFaceTo(mr.faceTo);
            host.TryChangeState<ClientMoveState>(st => st.DelayStop());
        }

        public override bool TestEnter()
        {
            return !host.dead;
        }

        public void SetPlayAnimOnce(int animCode)
        {
            this.playAnimationCode = animCode;
        }

        public void SetDurationOnce(float t)
        {
            this.duration = t;
        }

        protected override void OnEnter()
        {
            var anim = playAnimationCode;
            if (anim == 0)
            {
                anim = host.GetDefaultAnimation(Unit.ActionState.idle);
            }
            host.PlayAnimation(anim);
        }

        protected override bool OnUpdate()
        {
            return duration < 0 || passedTime < duration;
        }

        protected override void OnExit()
        {
            this.playAnimationCode = 0;
            this.duration = kDefaultDuration;
        }
    }
}
