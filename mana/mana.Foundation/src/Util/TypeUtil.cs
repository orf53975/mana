using System;
using System.Collections.Generic;
using System.Reflection;

namespace mana.Foundation
{
    public static class TypeUtil
    {
 
        public static void ForeachAllTypes(this AppDomain appDomain, Action<Type> action)
        {
            if (appDomain == null)
            {
                throw new NullReferenceException();
            }
            var ret = new List<Type>();
            foreach (var assembly in appDomain.GetAssemblies())
            {
                assembly.ForeachAllTypes(action);
            }
        }

        public static void ForeachAllTypes(this Assembly assembly, Action<Type> action)
        {
            if (assembly == null)
            {
                throw new NullReferenceException();
            }
            foreach (var type in assembly.GetTypes())
            {
                action.Invoke(type);
            }
        }

        public static List<Type> GetClassTypes<T>(this AppDomain appDomain, bool bContainAbstractClass = false)
        {
            if (appDomain == null)
            {
                throw new NullReferenceException();
            }
            var ret = new List<Type>();
            foreach (var assembly in appDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && (!type.IsAbstract || bContainAbstractClass) &&
                        typeof(T).IsAssignableFrom(type))
                    {
                        ret.Add(type);
                    }
                }
            }
            return ret;
        }


        public static List<Type> FindAllTypes(this AppDomain appDomain, Predicate<Type> match)
        {
            if (appDomain == null)
            {
                throw new NullReferenceException();
            }
            var ret = new List<Type>();
            foreach (var assembly in appDomain.GetAssemblies())
            {
                ret.AddRange(assembly.FindAllTypes(match));
            }
            return ret;
        }

        public static List<Type> FindAllTypes(this Assembly assembly, Predicate<Type> match)
        {
            if (assembly == null)
            {
                throw new NullReferenceException();
            }
            var ret = new List<Type>();
            foreach (var type in assembly.GetTypes())
            {
                if (match(type))
                {
                    ret.Add(type);
                }
            }
            return ret;
        }

        public static string LoadDll(AppDomain appDomain, string filePath)
        {
            try
            {
                filePath = Utils.GetAbsolutePath(filePath);
                var asm = Assembly.LoadFrom(filePath);
                if (ExistAssembly(appDomain, asm.FullName))
                {
                    appDomain.Load(asm.FullName);
                }
                return null;
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        private static bool ExistAssembly(AppDomain domain, string assemblyName)
        {
            Assembly[] asms = domain.GetAssemblies();
            for (int i = asms.Length - 1; i >= 0; i--)
            {
                if (asms[i].FullName.Equals(assemblyName))
                {
                    return true;
                }
            }
            return false;
        }

        public static T ParseArgs<T>(string[] args, Action<T> action , Action<string> parseArgvFailed = null)
        {
            var obj = Activator.CreateInstance<T>();
            var t = obj.GetType();
            foreach (var argv in args)
            {
                try
                {
                    if (argv.StartsWith("--"))
                    {
                        var kv = argv.Substring(2).Split('=');
                        if (kv.Length != 2)
                        {
                            continue;
                        }
                        var k = kv[0].Trim();
                        var v = kv[1].Trim();
                        var f = t.GetField(k);
                        if (f != null)
                        {
                            f.SetValue(obj, Convert.ChangeType(v, f.FieldType));
                        }

                        continue;
                    }
                    if(parseArgvFailed != null)
                    {
                        parseArgvFailed(argv);
                    }
                }
                catch
                {
                    if (parseArgvFailed != null)
                    {
                        parseArgvFailed(argv);
                    }
                }
            }
            if (action != null)
            {
                action.Invoke(obj);
            }
            return obj;
        }

    }
}