using xxd.battle;

namespace BattleSystem.Units
{
    public struct PropertyInfo
    {
        public int phyAtk;
        public int phyDef;
        public int magAtk;
        public int magDef;
        public int hpMax;
        public int mpMax;
        public int hit;
        public int dodge;
        public int crit;
        public int toughness;
        public int moveSpeed;

        public static PropertyInfo operator +(PropertyInfo b1, PropertyInfo b2)
        {
            return new PropertyInfo()
            {
                phyAtk      = b1.phyAtk + b2.phyAtk,
                phyDef      = b1.phyDef + b2.phyDef,
                magAtk      = b1.magAtk + b2.magAtk,
                magDef      = b1.magDef + b2.magDef,
                hpMax       = b1.hpMax + b2.hpMax,
                mpMax       = b1.mpMax + b2.mpMax,
                hit         = b1.hit + b2.hit,
                dodge       = b1.dodge + b2.dodge,
                crit        = b1.crit + b2.crit,
                toughness   = b1.toughness + b2.toughness,
                moveSpeed   = b1.moveSpeed + b2.moveSpeed
            };
        }

        public static PropertyInfo operator -(PropertyInfo b1, PropertyInfo b2)
        {
            return new PropertyInfo()
            {
                phyAtk      = b1.phyAtk - b2.phyAtk,
                phyDef      = b1.phyDef - b2.phyDef,
                magAtk      = b1.magAtk - b2.magAtk,
                magDef      = b1.magDef - b2.magDef,
                hpMax       = b1.hpMax - b2.hpMax,
                mpMax       = b1.mpMax - b2.mpMax,
                hit         = b1.hit - b2.hit,
                dodge       = b1.dodge - b2.dodge,
                crit        = b1.crit - b2.crit,
                toughness   = b1.toughness - b2.toughness,
                moveSpeed   = b1.moveSpeed - b2.moveSpeed
            };
        }

        public static implicit operator PropertyInfo(UnitProp data)
        {
            return new PropertyInfo()
            {
                phyAtk      = data.phyAtk,
                phyDef      = data.phyDef,
                magAtk      = data.magAtk,
                magDef      = data.magDef,
                hpMax       = data.hpMax,
                mpMax       = data.mpMax,
                hit         = data.hit,
                dodge       = data.dodge,
                crit        = data.crit,
                toughness   = data.toughness,
                moveSpeed   = data.moveSpeed
            };
        }
    }
}
