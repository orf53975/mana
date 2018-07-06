using BattleSystem.Units.Abilities;
using xxd.battle.opration;

namespace BattleSystem.Units.SM
{
    [StateConfig(priority = 3, actionState = Unit.ActionState.cast)]
    internal sealed class AbilityCast : State
    {
        public Ability ability = null;

        public Unit target;

        public AbilityCast(Unit unit) : base(unit) { }

        public void SetAbility(Ability ab)
        {
            this.ability = ab;
        }

        public void SetTarget(Unit u)
        {
            this.target = u;
        }

        public override void OnPlayerOperation(MoveRequest mr)
        {
            //if(movecancel)
            {
                //host.SetPosition(mr.x, mr.y, mr.z);
                //host.SetFaceTo(mr.faceTo);
                //host.TryChangeState<ClientMoveState>(st => st.DelayStop());
            }

            //throw new System.NotImplementedException();
        }

        public override void OnPlayerOperation(CastRequest cr)
        {
            //throw new System.NotImplementedException();
        }

        public override bool TestEnter()
        {
            return !host.dead && ability.TestCastTo(target);
        }

        protected override void OnEnter()
        {
            ability.CastTo(target);
        }

        protected override bool OnUpdate()
        {
            return ability.DoCasting();
        }

        protected override void OnExit()
        {
            ability = null;
            target = null;
        }
    }
}