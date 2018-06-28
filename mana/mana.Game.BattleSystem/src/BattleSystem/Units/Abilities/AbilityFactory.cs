using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using mana.Foundation;

namespace BattleSystem.Units.Abilities
{
    public static class AbilityFactory
    {
        #region <<Register AbilitiesTypeMap>>

        private static string AddAbilityType(Type type, Dictionary<int, Type> map)
        {
            var attr = type.TryGetAttribute<AbilityConfigAttribute>();
            if (attr == null)
            {
                Trace.TraceInformation("can't find AbilityDescAttribute in class:" + type.Name);
                return null;
            }
            if (map.ContainsKey(attr.tmplId))
            {
                return "id conflict " + map[attr.tmplId].Name + " - " + type.Name;
            }
            map.Add(attr.tmplId, type);
            return null;
        }

        static Dictionary<int, Type> _abilitiesTypeMap = null;
        static Dictionary<int, Type> AbilitiesTypeMap
        {
            get
            {
                if (_abilitiesTypeMap == null)
                {
                    _abilitiesTypeMap = new Dictionary<int, Type>();
                    var abTypes = AppDomain.CurrentDomain.FindAllTypes((type) => type.IsSubclassOf(typeof(Ability)));
                    var errInfo = new StringBuilder();
                    foreach (var t in abTypes)
                    {
                        var err = AddAbilityType(t, _abilitiesTypeMap);
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
                return _abilitiesTypeMap;
            }
        }

        #endregion

        public static Ability Creat(Unit unit, int tmplId, int lv)
        {
            Type findType;
            if (AbilitiesTypeMap.TryGetValue(tmplId, out findType))
            {
                var ab = Activator.CreateInstance(findType, unit) as Ability;
                ab.Level = lv;
                return ab;
            }
            return null;
        }

        public static T Creat<T>(Unit unit, int lv) where T : Ability
        {
            var attr = typeof(T).TryGetAttribute<AbilityConfigAttribute>();
            if (attr == null)
            {
                throw new Exception("can't find AbilityDescAttribute int class:" + typeof(T).Name);
            }
            return Creat(unit, attr.tmplId, lv) as T;
        }

    }
}