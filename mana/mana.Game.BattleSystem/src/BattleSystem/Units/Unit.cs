using BattleSystem.Lang;
using BattleSystem.Units.SM;
using mana.Foundation;
using System;
using System.Collections.Generic;
using xxd.battle;
using xxd.battle.opration;

namespace BattleSystem.Units
{
    public partial class Unit
    {

        public const float kDestoryDelay = 5.0f;

        public readonly BattleScene battle;

        readonly ExtendProperty<int> extProp;

        public int hpMax
        {
            get;
            private set;
        }

        public int hp
        {
            get;
            private set;
        }

        public float hpPercent
        {
            get
            {
                return (float)hp / hpMax;
            }
        }

        public int mpMax
        {
            get;
            private set;
        }

        public int mp
        {
            get;
            private set;
        }

        public float mpPercent
        {
            get
            {
                return (float)mp / mpMax;
            }
        }

        public float x
        {
            get;
            private set;
        }

        public float y
        {
            get;
            private set;
        }

        public float z
        {
            get;
            private set;
        }

        public float faceTo
        {
            get;
            private set;
        }

        public int phyAtk
        {
            get;
            private set;
        }

        public int phyDef
        {
            get;
            private set;
        }

        public int magAtk
        {
            get;
            private set;
        }

        public int magDef
        {
            get;
            private set;
        }

        public int hit
        {
            get;
            private set;
        }

        public int dodge
        {
            get;
            private set;
        }

        public int crit
        {
            get;
            private set;
        }

        public int toughness
        {
            get;
            private set;
        }

        public short moveSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 攻击加速
        /// </summary>
        public float atkAcceleration
        {
            get;
            private set;
        }

        /// <summary>
        /// 僵直加速
        /// </summary>
        public float stiffAcc
        {
            get;
            private set;
        }

        /// <summary>
        /// 沉默
        /// </summary>
        public bool silence
        {
            get;
            private set;
        }

        /// <summary>
        /// 眩晕
        /// </summary>
        public bool stun
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool invisibility
        {
            get;
            private set;
        }

        public float passedTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 能否复活
        /// </summary>
        public bool revivable
        {
            get;
            private set;
        }

        public bool dead
        {
            get;
            private set;
        }

        public bool destroyable
        {
            get
            {
                if (this.curState == null || !(this.curState is DeadState) || this.revivable)
                {
                    return false;
                }
                return this.curState.passedTime > kDestoryDelay;
            }
        }

        public Unit(BattleScene battle, UnitCreateData data)
        {
            this.battle = battle;
            this.baseInfo = data.info;
            this.baseProp = data.prop;
            this.extProp = new ExtendProperty<int>();
            this.InitUnit();
        }

        private void InitUnit()
        {
            this.InitState();
            this.InitAbilities();
            this.InitBuffs();
            this.InitProperty();
            this.InitExtProps();
            this.InitAiTree();
            this.InitPosition();
            this.InitHpMp();
        }


        public void ChangeHp(int value)
        {
            if (!dead)
            {
                hp = MathTools.Clamp(hp + value, 0, hpMax);
                if (hp == 0)
                {
                    this.dead = true;
                }
                battle.recorder.PushHpChange(this);
            }
        }

        /// <summary>
        /// 复活
        /// </summary>
        /// <param name="hpRestore"></param>
        public void Revival(int hpRestore = -1)
        {
            if (!this.dead)
            {
                throw new Exception("revival error! The Unit is not dead");
            }
            this.hp = hpRestore < 0 ? hpMax : hpRestore;
            this.dead = false;
        }


        private void SetProperty(PropertyInfo data)
        {
            this.phyAtk = data.phyAtk;
            this.phyDef = data.phyDef;
            this.magAtk = data.magAtk;
            this.magDef = data.magDef;
            this.hpMax  = data.hpMax;
            this.mpMax  = data.mpMax;
            this.hit    = data.hit;
            this.dodge  = data.dodge;
            this.crit   = data.crit;
            this.toughness = data.toughness;
            this.moveSpeed = (short)data.moveSpeed;
        }

        private void InitProperty()
        {
            PropertyInfo pi = this.baseProp;
            pi += CalAbilitiesPropertyBonus();
            pi += CalBuffsPropertyBonus();
            this.SetProperty(pi);
        }

        private void InitExtProps()
        {
            if (this.baseProp.extProps == null)
            {
                return;
            }
            foreach (var a in this.baseProp.extProps)
            {
                extProp.Set(a.key, a.value);
            }
        }

        private void InitPosition()
        {
            this.x = this.baseInfo.x;
            this.y = this.baseInfo.y;
            this.z = this.baseInfo.z;
        }
        
        private void InitHpMp()
        {
            var initHp = (int)(this.baseProp.hpMax * this.baseInfo.hpPercent + 0.5f);
            this.hp = MathTools.Clamp(initHp, 1, hpMax);
            var initMp = (int)(this.baseProp.hpMax * this.baseInfo.hpPercent + 0.5f);
            this.mp = MathTools.Clamp(initMp, 1, mpMax);
        }

        public void SetMaxHpMp()
        {
            hp = hpMax;
            mp = mpMax;
        }

        public bool IsHpPercentLessThan(float value)
        {
            return hpPercent < value;
        }

        public bool IsHpLessThan(int value)
        {
            return hp < value;
        }

        public bool IsHpPercentMoreThan(float value)
        {
            return hpPercent > value;
        }

        public bool IsHpMoreThan(int value)
        {
            return hp > value;
        }

        /// <summary>
        /// 取得下一个随机浮点数
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public float Roll(float max)
        {
            return battle.roll.NextFloat(max);
        }

        public float Roll(float min, float max)
        {
            var rv = battle.roll.NextFloat(max - min);
            return min + rv;
        }

        /// <summary>
        /// 目标是否是活着的敌人
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsLivingEnemy(Unit u)
        {
            return !u.dead && this.camp != u.camp;
        }

        /// <summary>
        /// 获得场景内所有活着的敌人
        /// </summary>
        /// <param name="ret"></param>
        public void GetAllLivingEnemys(List<Unit> ret)
        {
            battle.GetUnits(ret, IsLivingEnemy);
        }

        /// <summary>
        /// 获得视野内更近的单位
        /// </summary>
        /// <param name="u1"></param>
        /// <param name="u2"></param>
        /// <returns></returns>
        public Unit GetNearerUnitInFov(Unit u1, Unit u2)
        {
            var dis_sqr1 = u1 != null ? DistanceSqr(u1) : float.MaxValue;
            var dis_sqr2 = u2 != null ? DistanceSqr(u2) : float.MaxValue;
            if (dis_sqr1 < dis_sqr2)
            {
                return dis_sqr1 <= fov * fov ? u1 : null;
            }
            else
            {
                return dis_sqr2 <= fov * fov ? u2 : null;
            }
        }

        /// <summary>
        /// 获得视野范围内距离最近的敌人
        /// </summary>
        /// <returns></returns>
        public Unit FindNearestEnemyInFov()
        {
            Unit target = null;
            using (var enemies = ListCache<Unit>.Get())
            {
                this.GetAllLivingEnemys(enemies);
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    target = GetNearerUnitInFov(target, enemies[i]);
                }
            }
            return target;
        }

        public bool IsInArea(float area_x, float area_z, float area_range)
        {
            var d_x = Math.Abs(area_x - this.x);
            if (d_x > area_range)
            {
                return false;
            }
            var d_z = Math.Abs(area_z - this.z);
            if (d_z > area_range)
            {
                return false;
            }
            var sqr = d_x * d_x + d_z * d_z;
            return sqr <= area_range * area_range;
        }

        public bool IsTargetInRange(Unit u, float range)
        {
            var d_x = Math.Abs(u.x - this.x);
            if (d_x > range)
            {
                return false;
            }
            var d_y = Math.Abs(u.y - this.y);
            if (d_y > range)
            {
                return false;
            }
            var d_z = Math.Abs(u.z - this.z);
            if (d_z > range)
            {
                return false;
            }
            var sqr = d_x * d_x + d_y * d_y + d_z * d_z;
            return sqr <= range * range;
        }

        public float DistanceSqr(Unit u)
        {
            var d_x = (u.x - this.x);
            var d_y = (u.y - this.y);
            var d_z = (u.z - this.z);
            var sqr = d_x * d_x + d_y * d_y + d_z * d_z;
            return sqr;
        }

        public void SetPosition(float _x, float _y, float _z)
        {
            byte flag = 0;
            if (_x != this.x)
            {
                flag = BitFlag.AddByteFlag(flag, 0);
                this.x = _x;
            }
            if (_y != this.y)
            {
                flag = BitFlag.AddByteFlag(flag, 1);
                this.y = _y;
            }
            if (_z != this.z)
            {
                flag = BitFlag.AddByteFlag(flag, 2);
                this.z = _z;
            }
            if (flag != 0)
            {
                battle.recorder.PushPosition(this, flag);
            }
        }

        public void ChangePosition(float d_x, float d_y, float d_z)
        {
            byte flag = 0;
            if (d_x != 0)
            {
                flag = BitFlag.AddByteFlag(flag, 0);
                this.x += d_x;
            }
            if (d_y != 0)
            {
                flag = BitFlag.AddByteFlag(flag, 1);
                this.y += d_y;
            }
            if (d_z != 0)
            {
                flag = BitFlag.AddByteFlag(flag, 2);
                this.z += d_z;
            }
            if (flag != 0)
            {
                battle.recorder.PushPosition(this, flag);
            }
        }

        public int TryMoveToRangeInGround(Unit target, float range)
        {
            var d_x = (target.x - this.x);
            var d_y = (target.y - this.y);
            var d_z = (target.z - this.z);
            var dis_sqr_xz = d_x * d_x + d_z * d_z;
            var dis_xz = (float)Math.Sqrt(dis_sqr_xz);
            var mov = moveSpeed * battle.frameTime;
            if (dis_xz < mov)
            {
                this.SetPosition(target.x, 0, target.z);
                return d_y <= range ? 0 : -1;
            }
            var t = mov / dis_xz;
            this.ChangePosition(d_x * t, 0, d_z * t);
            if (IsTargetInRange(target, range))
            {
                return 0;
            }
            return 1;
        }

        public void TryMoveForward(float speed)
        {
            var dis = speed * battle.frameTime;
            var deg = this.faceTo * MathTools.Deg2Rad;
            var o_x = dis * (float)Math.Sin(deg);
            var o_z = dis * (float)Math.Cos(deg);
            this.ChangePosition(o_x, 0, o_z);
        }

        public void SetFaceTo(float nf)
        {
            if (faceTo == nf)
            {
                return;
            }
            this.faceTo = nf;
            battle.recorder.PushFaceTo(this);
        }

        public void LookAt(float target_x, float target_z)
        {
            var dz = target_z - this.z;
            var dx = target_x - this.x;

            if (dx == 0 && dz == 0)
            {
                return;
            }
            var nf = MathTools.Atan2ByDeg(dx, dz);
            this.SetFaceTo(nf);
        }

        public void LookAt(Unit target)
        {
            if (target != null)
            {
                LookAt(target.x, target.z);
            }
        }

        internal void OnPlayerOprationRequest(CastRequest cr)
        {
            if (this.curState == null)
            {
                this.TryChangeState<AbilityCast>(st =>
                {
                    st.SetAbility(this.TryGetAbility(cr.abilityId));
                    st.SetTarget(this.battle.Find(cr.targetId));
                });
            }
            else
            {
                this.curState.OnPlayerOperation(cr);
            }
        }

        internal void OnPlayerOprationRequest(MoveRequest mr)
        {
            if (this.curState == null)
            {
                this.SetPosition(mr.x, mr.y, mr.z);
                this.SetFaceTo(mr.faceTo);
                this.TryChangeState<ClientMoveState>(st => st.DelayStop());
            }
            else
            {
                this.curState.OnPlayerOperation(mr);
            }
        }
    }
}