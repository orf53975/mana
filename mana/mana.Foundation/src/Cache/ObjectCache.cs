using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace mana.Foundation
{
    public static class ObjectCache
    {
        #region <<class ObjectPoolManager>>

        private sealed class ObjectPoolManager
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

            internal void AddPool(Type t, IObjectPool pool)
            {
                try
                {
                    _lockSlim.EnterWriteLock();
                    _objectPools.Add(t, pool);
                }
                catch (Exception e)
                {
                    Logger.Exception(e);
                }
                finally
                {
                    _lockSlim.ExitWriteLock();
                }
            }

            internal void ClearAllPool()
            {
                try
                {
                    _lockSlim.EnterReadLock();
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
                    _lockSlim.ExitReadLock();
                }
            }

            internal string GetStateInfo()
            {
                try
                {
                    _lockSlim.EnterReadLock();
                    var sb = new StringBuilder("ObjectCache").AppendLine();
                    for (var iter = _objectPools.GetEnumerator(); iter.MoveNext();)
                    {
                        sb.AppendLine(iter.Current.Value.ToString());
                    }
                    return sb.ToString();
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
        }

        #endregion

        #region <<class ObjectPoolCache>>

        private static class ObjectPoolCache<T> where T : class, new()
        {
            private static readonly object locker = new object();

            private static ObjectPool<T> _pool = null;
            public static ObjectPool<T> Pool
            {
                get
                {
                    if (_pool == null)
                    {
                        lock (locker)
                        {
                            _pool = new ObjectPool<T>(() => Activator.CreateInstance<T>(), null, null, 16);
                            ObjectCache._poolManager.AddPool(typeof(T), _pool);
                        }
                    }
                    return _pool;
                }
            }

            public static T Get(Action<T> handler = null)
            {
                var obj = Pool.Get();
                if (handler != null)
                {
                    handler(obj);
                }
                return obj;
            }

            public static bool Put(T item)
            {
                return Pool.Put(item);
            }

            public static void Clear()
            {
                ((IObjectPool)Pool).Clear();
            }
        }

        #endregion

        static readonly ObjectPoolManager _poolManager = new ObjectPoolManager();

        public static T Get<T>(Action<T> handler = null)
            where T : class, new()
        {
            return ObjectPoolCache<T>.Get(handler);
        }

        public static object TryGet(Type t)
        {
            var pool = _poolManager.GetPool(t);
            if (pool != null)
            {
                return pool.Get();
            }
            return null;
        }


        public static bool Put<T>(T item)
            where T : class, new()
        {
            return ObjectPoolCache<T>.Put(item);
        }

        public static bool TryPut(object item)
        {
            var pool = _poolManager.GetPool(item.GetType());
            if (pool != null)
            {
                return pool.Put(item);
            }
            return false;
        }

        public static string GetStateInfo()
        {
            return _poolManager.GetStateInfo();
        }

        public static void Clear<T>()
            where T : class, new()
        {
            ObjectPoolCache<T>.Clear();
        }

        public static void Clear()
        {
            _poolManager.ClearAllPool();
        }
    }
}