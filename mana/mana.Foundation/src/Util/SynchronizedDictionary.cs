using System;
using System.Collections.Generic;
using System.Threading;

namespace mana.Foundation
{
    class SynchronizedDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();

        internal TValue GetValue(TKey key, TValue defaultValue = default(TValue))
        {
            try
            {
                _lockSlim.EnterReadLock();
                TValue value;
                if (_dictionary.TryGetValue(key, out value))
                {
                    return value;
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
            return defaultValue;
        }

        internal bool TryAdd(TKey key, TValue value)
        {
            try
            {
                _lockSlim.EnterWriteLock();
                _dictionary.Add(key, value);
                return true;
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
            return false;
        }

        public bool Remove(TKey key)
        {
            try
            {
                _lockSlim.EnterWriteLock();
                return _dictionary.Remove(key);
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
            return false;
        }

        internal List<TKey> GetKeys(List<TKey> toList = null)
        {
            try
            {
                _lockSlim.EnterReadLock();
                var count = _dictionary.Count;
                if (toList == null)
                {
                    toList = new List<TKey>(count);
                }
                else if (toList.Capacity < count)
                {
                    toList.Capacity = count;
                }
                for (var iter = _dictionary.GetEnumerator(); iter.MoveNext();)
                {
                    toList.Add(iter.Current.Key);
                }
                return toList;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
            return null;
        }

        internal void Foreach(Action<TKey, TValue> action)
        {
            try
            {
                _lockSlim.EnterReadLock();
                for (var iter = _dictionary.GetEnumerator(); iter.MoveNext();)
                {
                    action(iter.Current.Key, iter.Current.Value);
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

        internal void Foreach<TParam>(Action<TKey, TValue, TParam> action, TParam param)
        {
            try
            {
                _lockSlim.EnterReadLock();
                for (var iter = _dictionary.GetEnumerator(); iter.MoveNext();)
                {
                    action(iter.Current.Key, iter.Current.Value, param);
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


        internal void Clear()
        {
            try
            {
                _lockSlim.EnterWriteLock();
                _dictionary.Clear();
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
}
