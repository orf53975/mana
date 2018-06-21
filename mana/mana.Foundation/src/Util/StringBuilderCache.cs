using System;
using System.Text;

namespace mana.Foundation
{
    internal static class StringBuilderCache
    {
        private const int MAX_BUILDER_SIZE = 512;

        private static ObjectPool<StringBuilder> sbPool = new ObjectPool<StringBuilder>(() => new StringBuilder(), (sb) => sb.Length = 0, null, 32);

        public static StringBuilder Acquire(int capacity = 64)
        {
            var sb = sbPool.Get();
            if (sb.Capacity < capacity)
            {
                sb.Capacity = capacity;
            }
            return sb;
        }

        public static bool Release(StringBuilder sb)
        {
            if (sb.Capacity <= MAX_BUILDER_SIZE)
            {
                sbPool.Put(sb);
                return true;
            }
            return false;
        }

        public static string GetStringAndRelease(StringBuilder sb)
        {
            string result = sb.ToString();
            StringBuilderCache.Release(sb);
            return result;
        }
    }
}
