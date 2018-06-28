namespace BattleSystem.Units
{
    /// <summary>
    /// 显示特效
    /// </summary>
    public partial class Unit
    {
        public enum ShowEffectFlag : int
        {
            None            = 0,
            Bloodthirsty    = 1 << 0,
            Shield          = 1 << 1,
            ManaShield      = 1 << 2,
            MagicBrisk      = 1 << 3
        }

        public ShowEffectFlag EffectShow
        {
            get;
            private set;
        }

        public void ShowEffect(ShowEffectFlag flag, bool isShow)
        {
            var nset = isShow ? (EffectShow | flag) : (EffectShow & (~flag));
            if (nset != EffectShow)
            {
                EffectShow = nset;
                battle.recorder.PushEffectShow(this);
            }
        }
    }
}