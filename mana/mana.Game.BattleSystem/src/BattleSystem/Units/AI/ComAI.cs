 

namespace BattleSystem.Units.AI
{
    [AIConfig(tmplId = 0)]
    public class ComAI : AIEntity
    {
        public ComAI(Unit unit) : base(unit)
        {
        }

        public override UnitOrder SelectOrder()
        {
            if (host.IsInNullState())
            {
                return UnitOrder.Pool.GetIdleOrder(-1.0f);
            }
            return null;
        }
    }
}
