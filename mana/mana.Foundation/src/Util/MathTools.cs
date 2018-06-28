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

        public const float Deg2Rad = 0.0174532924F;
        public const float Rad2Deg = 57.29578F;

        public static bool IsInArea(float x, float y, float z, float area_x, float area_y, float area_z, float area_radius)
        {
            var d_x = Math.Abs(area_x - x);
            if (d_x > area_radius)
            {
                return false;
            }
            var d_y = Math.Abs(area_y - y);
            if (d_y > area_radius)
            {
                return false;
            }
            var d_z = Math.Abs(area_z - z);
            if (d_z > area_radius)
            {
                return false;
            }
            var sqr = d_x * d_x + d_y * d_y + d_z * d_z;
            return sqr <= area_radius * area_radius;
        }

        public static bool IsInArea(float x, float y, float area_x, float area_y, float area_radius)
        {
            var d_x = Math.Abs(area_x - x);
            if (d_x > area_radius)
            {
                return false;
            }
            var d_y = Math.Abs(area_y - y);
            if (d_y > area_radius)
            {
                return false;
            }
            var sqr = d_x * d_x + d_y * d_y;
            return sqr <= area_radius * area_radius;
        }

        public static float Atan2ByDeg(float x, float y)
        {
            return (float)(Math.Atan2(x, y) * Rad2Deg);
        }
    }
}
