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

            internal ObjectPool<T> GetPool<T>() where T : class, new()
            {
                var pool = GetPool(typeof(T));
                if (pool != null)
                {
                    return pool as ObjectPool<T>;
                }
                return null;
            }

            internal ObjectPool<T> AddPool<T>() where T : class, new()
            {
                try
                {
                    _lockSlim.EnterWriteLock();
                    var pool = new ObjectPool<T>(() => Activator.CreateInstance<T>(), null, null, 16);
                    _objectPools.Add(typeof(T), pool);
                    return pool;
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
                var pool = GetPool<T>();
                if (pool == null)
                {
                    pool = AddPool<T>();
                }
                return pool;
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

        static readonly ObjectPoolManager _poolManager = new ObjectPoolManager();

        public static T Get<T>(Action<T> handler = null)
            where T : class, new()
        {
            var pool = _poolManager.GetPool<T>();
            var item = pool != null ? pool.Get() : Activator.CreateInstance<T>();
            if (handler != null)
            {
                handler(item);
            }
            return item;
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
            return _poolManager.GetOrAddPool<T>().Put(item);
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
            var pool = _poolManager.GetPool(typeof(T));
            if (pool != null)
            {
                pool.Clear();
            }
        }

        public static void Clear()
        {
            _poolManager.ClearAllPool();
        }
    }
}