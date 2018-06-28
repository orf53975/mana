namespace BattleSystem.Units.SM
{
    [StateConfig(priority = 1, actionState = Unit.ActionState.move)]
    class MoveToTargetState : State
    {
        private Unit target;

        private float range;

        public MoveToTargetState(Unit unit) : base(unit){ }

        public override bool TestEnter()
        {
            return !host.dead;
        }

        public void SetTarget(Unit u)
        {
            this.target = u;
        }

        public void SetRange(float range)
        {
            this.range = range;
        }

        protected override void OnEnter()
        {
            var anim = host.GetDefaultAnimation(Unit.ActionState.move);
            host.PlayAnimation(anim);
        }

        protected override bool OnUpdate()
        {
            if (target == null)
            {
                return false;
            }
            host.LookAt(target);
            if (host.TryMoveToRangeInGround(target, range) != 0)
            {
                return true;
            }
            return false;
        }
    }
}
