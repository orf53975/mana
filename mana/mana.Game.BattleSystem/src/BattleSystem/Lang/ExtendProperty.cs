using System.Collections.Generic;

namespace BattleSystem.Lang
{
    class ExtendProperty<T>
    {
        readonly Dictionary<string, T> props = new Dictionary<string, T>();

        public T TryGet(string key)
        {
            T ret;
            if (props.TryGetValue(key, out ret))
            {
                return ret;
            }
            return default(T);
        }

        public void Set(string key, T value)
        {
            props[key] = value;
        }

        public bool Remove(string key)
        {
            return props.Remove(key);
        }
    }
}
