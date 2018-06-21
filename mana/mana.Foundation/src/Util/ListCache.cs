using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public static class ListCache<T>
    {
        private static readonly ObjectPool<_List> pool = new ObjectPool<_List>(
            () => new _List() , null , null , 16
        );

        public static _List Get()
        {
            var ret = pool.Get();
            ret.Clear();
            return ret;
        }

        private static bool Put(_List toRelease)
        {
            return pool.Put(toRelease);
        }

        public class _List : List<T>, IDisposable
        {
            void IDisposable.Dispose()
            {
                this.Clear();
                ListCache<T>.Put(this);
            }
        }
    }
}
