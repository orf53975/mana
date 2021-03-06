﻿using System;
using System.Collections.Generic;
using System.Text;

namespace mana.Foundation
{
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

        private T TryPopStack()
        {
            lock (_objects)
            {
                return _objects.Count > 0 ? _objects.Pop() : null;
            }
        }

        public T Get()
        {
            var ret = TryPopStack();
            if (ret != null)
            {
                if (_objectOnGet != null)
                {
                    _objectOnGet(ret);
                }
                return ret;
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
                if(_objects.Contains(item))
                {
                    Logger.Error("item had already add to pool!", item);
                    return false;
                }
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
            return string.Format("ObjectPool<{0}>[size = {1}]", typeof(T).Name, this._objects.Count);
        }

        public string GetAllObjectInfo()
        {
            var sb = new StringBuilder();
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