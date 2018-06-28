namespace BattleSystem.Units.Abilities
{
    public enum AbilityFlag : int
    {
        Default         = 0,
        PropertyBonus   = 1 << 0,
        ActiveUse       = 1 << 1,
        IgnoreStun      = 1 << 2,
        IgnoreSilence   = 1 << 3,
    }
}