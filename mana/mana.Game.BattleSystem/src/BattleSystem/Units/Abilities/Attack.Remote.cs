using BattleSystem.Util;
using xxd.battle;

namespace BattleSystem.Units.Abilities
{
    [AbilityConfig(tmplId = 1002, name = "远程攻击", flag = AbilityFlag.ActiveUse)]
    public sealed class RemoteAttack : Ability
    {
        public RemoteAttack(Unit unit) : base(unit) { }

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
        public static float kRemoteAttackBreak = 1.3f;
        public static float kRemoteAttackMin = 0.08f;
        public static float kRemoteAttackFloating = 0.03f;
        #endregion

        void DoAttack()
        {
            var dv = (host.phyAtk - target.phyDef) * kRemoteAttackBreak;
            var minDv = host.phyAtk * kRemoteAttackMin;
            if (dv > minDv)
            {
                dv += host.Roll(kRemoteAttackFloating) * dv;
            }
            else
            {
                dv = minDv;
            }
            //TODO
            //var bullet = BulletFactory.Creat(host, target, Bullet.MotionType.lockTarget);
            //bullet.SetDamageInfo(dv, Damage.dtype_physical , 0);
            //host.battle.AddBullet(bullet);
        }
    }
}