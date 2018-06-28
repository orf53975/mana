using System.Collections.Generic;

namespace BattleSystem.Lang
{
    internal class ExtendAttribute
    {
        readonly Dictionary<string, object> attributes = new Dictionary<string, object>();

        public T TryGetAttribute<T>(string key)
        {
            object ret;
            if (attributes.TryGetValue(key, out ret))
            {
                return (T)ret;
            }
            return default(T);
        }

        public void SetAttribute(string key, object value)
        {
            if (value != null)
            {
                attributes[key] = value;
            }
            else
            {
                attributes.Remove(key);
            }
        }
    }
}
