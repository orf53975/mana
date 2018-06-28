using System;
using System.Collections.Generic;
using System.Text;
using mana.Foundation;

namespace BattleSystem.Units.Buffs
{
    public static class BuffFactory
    {
        #region <<Register BuffsTypeMap>>

        static string AddBuffType(Type type, Dictionary<int, Type> registerTable)
        {
            var attr = type.TryGetAttribute<BuffConfigAttribute>();
            if (attr == null)
            {
                return "can't find buff int class:" + type.Name;
            }
            if (registerTable.ContainsKey(attr.tmplId))
            {
                return "id conflict " + registerTable[attr.tmplId].Name + " - " + type.Name;
            }
            registerTable.Add(attr.tmplId, type);
            return null;
        }

        static Dictionary<int, Type> _buffsTypeMap = null;
        static Dictionary<int, Type> BuffsTypeMap
        {
            get
            {
                if (_buffsTypeMap == null)
                {
                    _buffsTypeMap = new Dictionary<int, Type>();

                    var buffTypes = AppDomain.CurrentDomain.FindAllTypes((type) => type.IsSubclassOf(typeof(Buff)));
                    var errInfo = new StringBuilder();
                    foreach (var t in buffTypes)
                    {
                        var err = AddBuffType(t, _buffsTypeMap);
                        if (err != null)
                        {
                            errInfo.AppendLine(err);
                        }
                    }
                    if (errInfo.Length > 0)
                    {
                        throw new Exception(errInfo.ToString());
                    }
                }
                return _buffsTypeMap;
            }
        }
        #endregion

        public static Buff Creat(int tmplId, int lv)
        {
            Type findType;
            if (BuffsTypeMap.TryGetValue(tmplId, out findType))
            {
                var ret = Activator.CreateInstance(findType, lv);
                return ret as Buff;
            }
            return null;
        }

        public static T Creat<T>(int lev) where T : Buff
        {
            var attr = typeof(T).TryGetAttribute<BuffConfigAttribute>();
            if (attr == null)
            {
                throw new Exception("can't find BuffConfigAttribute int class:" + typeof(T).Name);
            }
            return Creat(attr.tmplId, lev) as T;
        }

    }
}