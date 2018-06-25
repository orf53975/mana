using System;

namespace mana.Foundation
{
    public static class TimeUtil
    {
        static readonly DateTime ReferenceTime = new DateTime(2018, 1, 1);

        public static int GetCurrentTime()
        {
            var ts = DateTime.Now - ReferenceTime;
            return (int)ts.TotalSeconds;
        }

        public static int GetTimeSpan(int curTimeTick, int preTimeTick)
        {
            if (curTimeTick < 0 && preTimeTick > 0)
            {
                long ret = curTimeTick & uint.MaxValue;
                return (int)(ret - preTimeTick);
            }
            return curTimeTick - preTimeTick;
        }
    }
}