namespace BattleSystem.Units.SM
{
    [StateConfig(priority = int.MaxValue, actionState = Unit.ActionState.dead)]
    class DeadState : State
    {
        private int animationCode;

        public DeadState(Unit unit) : base(unit) { }

        public void SetPlayAnimOnce(int animCode)
        {
            this.animationCode = animCode;
        }

        public override bool TestEnter()
        {
            return host.dead;
        }

        protected override void OnEnter()
        {
            var anim = animationCode;
            if (anim == 0)
            {
                anim = host.GetDefaultAnimation(Unit.ActionState.dead);
            }
            host.PlayAnimation(anim);
        }

        protected override bool OnUpdate()
        {
            return host.dead;
        }

        protected override void OnExit()
        {
            animationCode = 0;
        }
    }
}
