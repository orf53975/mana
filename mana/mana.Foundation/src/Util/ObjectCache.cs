using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public static class ObjectCache
    {
        #region <<class ObjectCacheImpl>>

        private sealed class ObjectCacheImpl
        {
            private readonly Dictionary<Type, IObjectPool> _objectPools = new Dictionary<Type, IObjectPool>();

            internal ObjectPool<T> GetPool<T>() 
                where T : class, new()
            {
                var pool = GetPool(typeof(T));
                if (pool != null)
                {
                    return pool as ObjectPool<T>;
                }
                return null;
            }

            internal IObjectPool GetPool(Type t)
            {
                IObjectPool pool = null;
                lock (_objectPools)
                {
                    if (_objectPools.TryGetValue(t, out pool))
                    {
                        return pool;
                    }
                }
                return null;
            }

            internal ObjectPool<T> AddPool<T>()
                where T : class, new()
            {
                var pool = new ObjectPool<T>(() => Activator.CreateInstance<T>(), null, null, 16);
                lock (_objectPools)
                {
                    _objectPools.Add(typeof(T), pool);
                }
                return pool;
            }

            internal void ClearAll()
            {
                lock (_objectPools)
                {
                    for (var iter = _objectPools.GetEnumerator(); iter.MoveNext();)
                    {
                        iter.Current.Value.Clear();
                    }
                }
            }
        }

        #endregion

        private static readonly ObjectCacheImpl _instance = new ObjectCacheImpl();

        public static T Get<T>() 
            where T : class, Cacheable, new()
        {
            var pool = _instance.GetPool<T>();
            if (pool == null)
            {
                return Activator.CreateInstance<T>();
            }
            return pool.Get();
        }

        public static T Get<T>(Action<T> handler) 
            where T : class, Cacheable, new()
        {
            var obj = Get<T>();
            if (handler != null)
            {
                handler(obj);
            }
            return obj;
        }

        internal static object Get(Type t)
        {
            var pool = _instance.GetPool(t);
            if (pool == null)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return pool.Get();
            }
        }

        public static bool Put<T>(T obj) 
            where T : class, Cacheable, new()
        {
            if (obj is Cacheable)
            {
                var pool = _instance.GetPool<T>();
                if (pool == null)
                {
                    pool = _instance.AddPool<T>();
                }
                return pool.Put(obj);
            }
            return false;
        }

        public static void Clear<T>()
            where T : class, Cacheable, new()
        {
            var pool = _instance.GetPool(typeof(T));
            if (pool != null)
            {
                pool.Clear();
            }
        }

        public static void Clear()
        {
            _instance.ClearAll();
        }
    }
}
