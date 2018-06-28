using xxd.sync;

namespace BattleSystem.Units.Abilities
{
    [AbilityConfig(tmplId = 1001, name = "近战攻击", flag = AbilityFlag.ActiveUse)]
    public sealed class MeleeAttack : Ability
    {
        public MeleeAttack(Unit unit) : base(unit) { }

        private float totalTime;

        private float passTime;

        private bool isAttacked;

        protected override void InitCast()
        {
            this.totalTime = host.atkTimeLength / (1.0f + host.atkAcceleration);
            this.passTime = 0;
            this.isAttacked = false;

            var anim = host.GetDefaultAnimation(Unit.ActionState.attack);
            this.host.PlayAnimation(anim, totalTime);
            this.host.LookAt(target);
        }

        public override bool DoCasting()
        {
            passTime += host.battle.frameTime;
            if (!isAttacked && passTime > host.atkKeyFrameTime * totalTime)
            {
                isAttacked = true;
                DoAttack();
            }
            return passTime < totalTime;
        }

        #region <<const config>>
        public static float kMeleeAttackBreak = 1.2f;
        public static float kMeleeAttackMin = 0.1f;
        public static float kMeleeAttackFloating = 0.05f;
        #endregion

        void DoAttack()
        {
            if (target == null || !host.IsTargetInRange(target, host.atkRange + target.bodySize))
            {
                return;
            }
            var dv = (host.phyAtk - target.phyDef) * kMeleeAttackBreak;
            var minDv = host.phyAtk * kMeleeAttackMin;
            if (dv > minDv)
            {
                dv += host.Roll(kMeleeAttackFloating) * dv;
            }
            else
            {
                dv = minDv;
            }
            host.Attack(target, dv, Damage.dtype_physical);
        }
    }
}