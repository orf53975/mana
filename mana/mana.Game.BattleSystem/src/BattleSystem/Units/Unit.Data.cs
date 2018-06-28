using mana.Foundation;
using xxd.sync;

namespace BattleSystem.Units
{
    public partial class Unit
    {
        public enum Camp : sbyte { undefined = -1, none = 0, camp1 = 1, camp2 = 2 }

        public enum UnitType : sbyte { undefined = -1, normal = 0, hero = 1, elite = 2, boss = 3 }

        public const int EmptyPlayerID = 0;

        public readonly UnitInfo baseInfo;

        public readonly UnitProp baseProp;

        public int ownerPlayerId
        {
            get
            {
                return baseInfo.playerId;
            }
        }

        public int uid
        {
            get
            {
                return baseInfo.uid;
            }
        }

        public UnitType unitType
        {
            get
            {
                return (UnitType)baseInfo.type;
            }
        }

        public Camp camp
        {
            get
            {
                var ret = (Camp)baseInfo.camp;
                return ret;
            }
        }


        public string appearance
        {
            get
            {
                return baseInfo.appearance;
            }
        }


        public byte category
        {
            get
            {
                return baseInfo.category;
            }
        }

        public string name
        {
            get
            {
                return baseInfo.name;
            }
        }

        public float fov
        {
            get
            {
                return baseProp.fov;
            }
        }

        public float atkRange
        {
            get
            {
                return baseProp.atkRange;
            }
        }

        public float bodySize
        {
            get
            {
                return baseProp.bodySize;
            }
        }

        public float atkTimeLength
        {
            get
            {
                return baseProp.atkTimeLength;
            }
        }

        /// <summary>
        /// 攻击前摇时间
        /// </summary>
        public float atkKeyFrameTime
        {
            get
            {
                return baseProp.atkKeyFrameTime;
            }
        }

        /// <summary>
        /// 受伤僵直时间
        /// </summary>
        public float stiffTime
        {
            get
            {
                return baseProp.stiffTime;
            }
        }

        public short lev
        {
            get
            {
                return baseInfo.lev;
            }
        }


        public float startX
        {
            get
            {
                return baseInfo.x;
            }
        }

        public float startY
        {
            get
            {
                return baseInfo.y;
            }
        }

        public float startZ
        {
            get
            {
                return baseInfo.z;
            }
        }

        public UnitInfo GetSnapData()
        {
            var ret = ObjectCache.Get<UnitInfo>();
            ret.playerId = this.ownerPlayerId;
            ret.uid = this.uid;
            ret.name = this.name;
            ret.appearance = this.appearance;
            ret.category = this.category;
            ret.type = (byte)this.unitType;
            ret.camp = (byte)this.camp;
            ret.lev = this.lev;
            ret.x = this.x;
            ret.y = this.y;
            ret.z = this.z;
            ret.faceTo = this.faceTo;
            ret.hpPercent = this.hpPercent;
            ret.mpPercent = this.mpPercent;
            ret.moveSpeed = this.moveSpeed;
            ret.actionState = (byte)this.curActionState;
            ret.currentAnim = this.curAnimation;
            ret.effectShow = (int)this.EffectShow;

            //TODO ret.controlFlag = 
            //TODO ret.buffsData = 
            return ret;
        }

    }
}