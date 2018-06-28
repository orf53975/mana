namespace BattleSystem.Units.SM
{
    [StateConfig(priority = 9, actionState = Unit.ActionState.injury)]
    class InjuryState : State
    {
        private int playAnimationCode = 0;

        private float duration;

        public InjuryState(Unit unit) : base(unit) { }

        public override bool TestEnter()
        {
            return !host.dead;
        }

        public void SetPlayAnimOnce(int animCode)
        {
            this.playAnimationCode = animCode;
        }

        protected override void OnEnter()
        {
            var anim = playAnimationCode;
            if (anim == 0)
            {
                anim = host.GetDefaultAnimation(Unit.ActionState.injury);
            }
            this.duration = host.stiffTime / (1.0f + host.stiffAcc);
            host.PlayAnimation(anim, duration);
        }

        protected override bool OnUpdate()
        {
            return passedTime < duration;
        }

        protected override void OnExit()
        {
            this.playAnimationCode = 0;
        }
    }
}
