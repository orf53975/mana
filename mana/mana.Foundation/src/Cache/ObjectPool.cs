using System;
using System.Collections.Generic;
using System.Text;

namespace mana.Foundation
{
    #region <<definition interface IObjectPool>>
    public interface IObjectPool
    {
        object Get();
        bool Put(object item);
        void Clear();
    }
    #endregion

    public class ObjectPool<T> : IObjectPool where T : class
    {
        private readonly Stack<T>   _objects = new Stack<T>();
        private readonly Func<T>    _objectGenerator;
        private readonly Action<T>  _objectOnGet;
        private readonly Action<T>  _objectOnRelease;

        private int mCapacity; 

        public ObjectPool(Func<T> objectGenerator, Action<T> objectOnGet = null, Action<T> objectOnRelease = null , int capacity = 16)
        {
            _objectGenerator = objectGenerator;
            _objectOnGet = objectOnGet;
            _objectOnRelease = objectOnRelease;
            this.ChangeCapacity(capacity);
        }

        public void ChangeCapacity(int newCapacity)
        {
            this.mCapacity = MathTools.Clamp(newCapacity, 2, 8192);
        }

        public T Get()
        {
            lock (_objects)
            {
                if (_objects.Count > 0)
                {
                    var obj = _objects.Pop();
                    if (_objectOnGet != null)
                    {
                        _objectOnGet(obj);
                    }
                    return obj;
                }
            }
            if (_objectGenerator != null)
            {
                return _objectGenerator();
            }
            return null;
        }

        public bool Put(T item)
        {
            if (item == null)
            {
                Logger.Error("ObjectPool<{0}> put null!", typeof(T).FullName);
                return false;
            }
            if (_objects.Count > mCapacity)
            {
                Logger.Error("ObjectPool<{0}> is full!! capacity = {1}", typeof(T).FullName, mCapacity);
                return false;
            }
            lock (_objects)
            {
                _objects.Push(item);
            }
            if (_objectOnRelease != null)
            {
                _objectOnRelease(item);
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            lock (_objects)
            {
                foreach (T o in _objects)
                {
                    sb.Append(o.ToString()).Append('\n');
                }
            }
            return sb.ToString();
        }

        #region <<Implement IObjectPool>>
        object IObjectPool.Get()
        {
            return this.Get();
        }

        bool IObjectPool.Put(object item)
        {
            return this.Put(item as T);
        }

        void IObjectPool.Clear()
        {
            lock (_objects)
            {
                _objects.Clear();
            }
        }
        #endregion
    }
}