using System;
using System.Collections.Generic;
using System.Threading;

namespace mana.Foundation
{
    public static class ObjectCache
    {
        #region <<class ObjectCacheImpl>>

        private sealed class ObjectCacheImpl
        {
            private readonly Dictionary<Type, IObjectPool> _objectPools = new Dictionary<Type, IObjectPool>();

            private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();

            internal IObjectPool GetPool(Type t)
            {
                try
                {
                    _lockSlim.EnterReadLock();
                    IObjectPool pool;
                    if (_objectPools.TryGetValue(t, out pool))
                    {
                        return pool;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                    return null;
                }
                finally
                {
                    _lockSlim.ExitReadLock();
                }
            }

            internal ObjectPool<T> GetPool<T>() where T : class, new()
            {
                var pool = GetPool(typeof(T));
                if (pool != null)
                {
                    return pool as ObjectPool<T>;
                }
                return null;
            }

            private ObjectPool<T> AddPool<T>() where T : class, new()
            {
                try
                {
                    _lockSlim.EnterWriteLock();
                    var ret = new ObjectPool<T>(() => Activator.CreateInstance<T>(), null, null, 16);
                    _objectPools.Add(typeof(T), ret);
                    return ret;
                }
                catch (Exception e)
                {
                    Logger.Exception(e);
                    return null;
                }
                finally
                {
                    _lockSlim.ExitWriteLock();
                }
            }

            internal ObjectPool<T> GetOrAddPool<T>() where T : class, new()
            {
                var ret = GetPool<T>();
                if (ret == null)
                {
                    ret = AddPool<T>();
                }
                return ret;
            }

            internal void ClearAll()
            {
                try
                {
                    _lockSlim.EnterWriteLock();
                    for (var iter = _objectPools.GetEnumerator(); iter.MoveNext();)
                    {
                        iter.Current.Value.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }
                finally
                {
                    _lockSlim.ExitWriteLock();
                }
            }
        }

        #endregion

        private static readonly ObjectCacheImpl _instance = new ObjectCacheImpl();

        public static T Get<T>() 
            where T : class, ICacheable, new()
        {
            var pool = _instance.GetPool<T>();
            if (pool == null)
            {
                return Activator.CreateInstance<T>();
            }
            return pool.Get();
        }

        public static T Get<T>(Action<T> handler) 
            where T : class, ICacheable, new()
        {
            var obj = Get<T>();
            if (handler != null)
            {
                handler(obj);
            }
            return obj;
        }

        internal static object TryGet(Type t)
        {
            var pool = _instance.GetPool(t);
            if (pool != null)
            {
                return pool.Get();
            }
            return null;
        }

        public static bool Put<T>(T obj) 
            where T : class, ICacheable, new()
        {
            return _instance.GetOrAddPool<T>().Put(obj);
        }

        public static void Clear<T>()
            where T : class, ICacheable, new()
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
