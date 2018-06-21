using System;

namespace mana.Foundation
{
    public static class MathTools
    {
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        public static float NextFloat(this Random ran, float maxValue)
        {
            return ran.Next(10000) * 0.0001f * maxValue;
        }
    }
}
