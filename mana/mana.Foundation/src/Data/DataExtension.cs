using System;

namespace mana.Foundation
{
    public static class DataExtension
    {
        public static void DeepCopyTo<T>(this T src, T dst) 
            where T : class, ISerializable, new()
        {
            if (src == null || dst == null)
            {
                throw new NullReferenceException();
            }
            using (var buf = ByteBuffer.Pool.Get())
            {
                src.Encode(buf);
                dst.Decode(buf);
            }
        }

        public static T DeepClone<T>(this T src)
            where T : class, ISerializable, new()
        {
            var __t = typeof(T);
            var obj = ObjectCache.TryGet(__t);
            if (obj == null)
            {
                obj = Activator.CreateInstance(__t);
            }
            var ret = obj as T;
            src.DeepCopyTo(ret);
            return ret;
        }

        public static void ReleaseToCache(this ICacheable[] arr)
        {
            if (arr == null)
            {
                return;
            }
            for(int i = 0; i < arr.Length; i++)
            {
                arr[i].ReleaseToCache();
                arr[i] = null;
            }
        }

        #region <<Format Extension>>
        public static string ToFormatString(this IFormatString[] arr, string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append('{');
            if (arr != null)
            {
                var newIndent = newLineIndent + "\t";
                for (int i = 0; i < arr.Length; i++)
                {
                    sb.Append("\r\n").Append(newIndent).Append(arr[i].ToFormatString(newIndent));
                    if (i != arr.Length - 1)
                    {
                        sb.Append(',');
                    }
                }
            }
            else
            {
                sb.Append("null");
            }
            sb.Append("\r\n").Append(newLineIndent).Append('}');
            return StringBuilderCache.GetAndRelease(sb);
        }


        public static string ToFormatStr<T>(T[] arr, string newLineIndent) where T : IComparable
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append('{');
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    sb.Append(arr[i]);
                    if (i != arr.Length - 1)
                    {
                        sb.Append(',');
                    }
                }
            }
            else
            {
                sb.Append("null");
            }
            sb.Append('}');
            return StringBuilderCache.GetAndRelease(sb);
        }

        #endregion

    }
}
