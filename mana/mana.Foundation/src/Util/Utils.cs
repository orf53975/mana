using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace mana.Foundation
{
    public static class Utils
    {
        private static string _currentDirectory = null;
        public static string CurrentDirectory
        {
            get
            {
                if (_currentDirectory == null)
                {
                    //_currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                    _currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                }
                return _currentDirectory;
            }
        }

        public static string AdjustFilePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(CurrentDirectory, filePath);
            }
            return filePath;
        }

        /// <summary>
        /// 解析地址格式
        ///     127.0.0.1:8082
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static IPEndPoint GetIPEndPoint(string address)
        {
            try
            {
                var strs = address.Split(':');
                var ipad = IPAddress.Parse(strs[0]);
                var port = int.Parse(strs[1]);
                return new IPEndPoint(ipad, port);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                return null;
            }
        }

        public static List<K> GetKeys<K, V>(this Dictionary<K, V> dic, List<K> toList = null)
        {
            var count = dic.Count;
            if (toList == null)
            {
                toList = new List<K>(count);
            }
            else if (toList.Capacity < count)
            {
                toList.Capacity = count;
            }
            for (var iter = dic.GetEnumerator(); iter.MoveNext();)
            {
                toList.Add(iter.Current.Key);
            }
            return toList;
        }

        public static List<V> GetValues<K, V>(this Dictionary<K, V> dic, List<V> toList = null)
        {
            var count = dic.Count;
            if (toList == null)
            {
                toList = new List<V>(count);
            }
            else if (toList.Capacity < count)
            {
                toList.Capacity = count;
            }
            for (var iter = dic.GetEnumerator(); iter.MoveNext();)
            {
                toList.Add(iter.Current.Value);
            }
            return toList;
        }

        public static T[] Concat<T>(T[] a, T b)
        {
            var ret = new T[a.Length + 1];
            Array.Copy(a, ret, a.Length);
            ret[a.Length] = b;
            return ret;
        }

        public static TA TryGetAttribute<TA>(this Type type) where TA : Attribute
        {
            var attrs = type.GetCustomAttributes(typeof(TA), false);
            if (attrs.Length > 0)
            {
                return (attrs[0] as TA);
            }
            return null;
        }

        public static string ToHexString(byte[] data)
        {
            var sb = StringBuilderCache.Acquire();
            for (int i = 0; i < data.Length; i++)
            {
                byte d = data[i];
                sb.Append(d.ToString("x2"));
            }
            return StringBuilderCache.GetAndRelease(sb);
        }

        public static string CalculateMD5(Stream stream)
        {
            var md5Hasher = System.Security.Cryptography.MD5.Create();
            md5Hasher.ComputeHash(stream);
            return ToHexString(md5Hasher.Hash);
        }

        public static string CalculateMD5(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return CalculateMD5(ms);
            }
        }

        public static int GetRemotePort(Socket socket)
        {
            if (socket != null)
            {
                var temp = socket.RemoteEndPoint.ToString().Split(':')[1];
                int port = Convert.ToInt32(temp);
                return port;
            }
            return 0;
        }

        public static T ToEnum<T>(string val, T defaultValue) where T : struct, System.IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

            try
            {
                T result = (T)Enum.Parse(typeof(T), val, true);
                return result;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T ToEnum<T>(int val, T defaultValue) where T : struct, IConvertible
        {
            var et = typeof(T);
            if (!et.IsEnum)
            {
                throw new System.ArgumentException("T must be an enumerated type");
            }
            object obj = val;
            if (Enum.IsDefined(et, obj))
            {
                return (T)obj;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int BinarySearch<T>(List<T> lists, Func<T, long> compareFunc)
        {
            int start = 0, end = lists.Count - 1, mid;
            long v;
            while (start <= end)
            {
                mid = start + (end - start >> 1);
                v = compareFunc(lists[mid]);
                if (v == 0)
                {
                    return mid;
                }
                if (v < 0)
                {
                    start = mid + 1;
                }
                else if (v > 0)
                {
                    end = mid - 1;
                }
            }
            return ~start;
        }

        public static int BinarySearch<T, TV>(List<T> lists, TV value, Func<T, TV, long> compareFunc)
        {
            int start = 0, end = lists.Count - 1, mid;
            long v;
            while (start <= end)
            {
                mid = start + (end - start >> 1);
                v = compareFunc(lists[mid], value);
                if (v == 0)
                {
                    return mid;
                }
                if (v < 0)
                {
                    start = mid + 1;
                }
                else if (v > 0)
                {
                    end = mid - 1;
                }
            }
            return ~start;
        }
    }
}