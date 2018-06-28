namespace BattleSystem.Units.AI
{
    public abstract class AIEntity
    {
        public readonly Unit host;
        public AIEntity(Unit unit) { this.host = unit; }

        public abstract UnitOrder SelectOrder();
    }
}
