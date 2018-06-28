namespace BattleSystem.Units.Buffs
{
    public enum BuffFlag : int
    {
        Default = 0,

        /// <summary>
        /// 有属性加成
        /// </summary>
        PropertyBonus = 1 << 0,

        /// <summary>
        /// 不可驱散的
        /// </summary>
        NoDispellable = 1 << 1,

        /// <summary>
        /// 可叠加的
        /// </summary>
        Stackable = 1 << 2,
    }
}