using System;
using System.IO;
using System.Text;

namespace mana.Foundation.Network.Sever
{
    public static class ConfigUtil
    {

        public static void Save<T>(string relativePath, T cfgObj, Func<T, string> serializer) where T : class, new()
        {
            var filePath = Utils.GetAbsolutePath(relativePath);
            var dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var str = serializer(cfgObj);
            File.WriteAllText(filePath, str, Encoding.UTF8);
        }

        public static T Load<T>(string relativePath, Func<string, T> deserializer) where T : class, new()
        {
            var filePath = Utils.GetAbsolutePath(relativePath);
            if (File.Exists(filePath))
            {
                var str = File.ReadAllText(filePath);
                var ret = deserializer(str);
                return ret;
            }
            return null;
        }

        public static T LoadOrNewSave<T>(string relativePath, Func<string, T> deserializer, Func<T, string> serializer) where T : class, new()
        {
            var ret = Load<T>(relativePath, deserializer);
            if (ret != null)
            {
                return ret;
            }
            ret = Activator.CreateInstance<T>();
            Save<T>(relativePath, ret, serializer);
            return ret;
        }
    }
}
