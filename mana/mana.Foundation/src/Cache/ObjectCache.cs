using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace mana.Foundation
{
    public static class ObjectCache
    {
        #region <<Class ObjectCache.Cache>>

        private sealed class Cache<T> : ObjectPool<T> where T : class
        {
            private static Cache<T> _instance = null;
            public static Cache<T> Instance
            {
                get
                {
                    return _instance;
                }
            }

            public static bool CreateInstance()
            {
                if (_instance == null)
                {
                    _instance = new Cache<T>(() => Activator.CreateInstance<T>(), null, null, 16);
                    return true;
                }
                return false;
            }

            private Cache(Func<T> objectGenerator, Action<T> objectOnGet = null, Action<T> objectOnRelease = null, int capacity = 16)
                : base(objectGenerator, objectOnGet, objectOnRelease, capacity)
            {
            }

            public static T TryGet()
            {
                return _instance != null ? _instance.Get() : null;
            }

            public static bool TryPut(T item)
            {
                return _instance != null && _instance.Put(item);
            }
        }

        #endregion

        static readonly SynchronizedDictionary<Type, IObjectPool> poolManager = new SynchronizedDictionary<Type, IObjectPool>();

        static readonly object _lockCreateTypeCache = new object();

        public static T Get<T>(Action<T> handler = null)
            where T : class, new()
        {
            T item = Cache<T>.TryGet();
            if (item == null)
            {
                item = Activator.CreateInstance<T>();
            }
            if (handler != null)
            {
                handler(item);
            }
            return item;
        }

        public static object TryGet(Type t)
        {
            var pool = poolManager.GetValue(t);
            if (pool != null)
            {
                return pool.Get();
            }
            return null;
        }

        public static bool Put<T>(T item)
            where T : class, new()
        {
            if (Cache<T>.Instance == null)
            {
                lock (_lockCreateTypeCache)
                {
                    if (Cache<T>.CreateInstance()) { poolManager.TryAdd(typeof(T), Cache<T>.Instance); }
                }
            }
            return Cache<T>.TryPut(item);
        }

        public static bool TryPut(object item)
        {
            var pool = poolManager.GetValue(item.GetType());
            if (pool != null)
            {
                return pool.Put(item);
            }
            return false;
        }

        public static string GetStateInfo()
        {
            var sb = StringBuilderCache.Acquire();
            poolManager.Foreach((t, p, param) =>
            {
                param.AppendLine(p.ToString());
            }, sb);
            return sb.ToString();
        }

        public static void Clear<T>()
            where T : class, new()
        {
            var pool = poolManager.GetValue(typeof(T));
            if (pool != null)
            {
                pool.Clear();
            }
        }

        public static void Clear()
        {
            poolManager.Foreach((t, p) =>
            {
                p.Clear();
            });
        }
    }
}