namespace BattleSystem.Units.SM
{
    [StateConfig(priority = 0, openAI = true , actionState = Unit.ActionState.move)]
    class WanderState : State
    {
        private float targetX;

        private float targetZ;

        public WanderState(Unit unit) : base(unit) { }

        public void SetTarget(float target_x, float target_z)
        {
            this.targetX = target_x;
            this.targetZ = target_z;
        }

        public override bool TestEnter()
        {
            return !host.dead;
        }

        protected override void OnEnter()
        {
            var anim = host.GetDefaultAnimation(Unit.ActionState.move);
            host.PlayAnimation(anim);
            host.LookAt(targetX, targetZ);
        }

        protected override bool OnUpdate()
        {
            if (!host.IsInArea(targetX, targetZ, 1.0f))
            {
                host.TryMoveForward(3.0f);
                return true;
            }
            return false;
        }
    }
}
