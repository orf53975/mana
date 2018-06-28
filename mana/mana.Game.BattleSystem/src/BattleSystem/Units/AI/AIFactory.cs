using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using mana.Foundation;

namespace BattleSystem.Units.AI
{
    class AIFactory
    {
        #region <<Register AITypeMap>>

        private static string AddAIType(Type type, Dictionary<int, Type> map)
        {
            var attr = type.TryGetAttribute<AIConfigAttribute>();
            if (attr == null)
            {
                Trace.TraceInformation("can't find AIConfigAttribute in class:" + type.Name);
                return null;
            }
            if (map.ContainsKey(attr.tmplId))
            {
                return "id conflict " + map[attr.tmplId].Name + " - " + type.Name;
            }
            map.Add(attr.tmplId, type);
            return null;
        }

        static Dictionary<int, Type> _aiTypeMap = null;
        static Dictionary<int, Type> AITypeMap
        {
            get
            {
                if (_aiTypeMap == null)
                {
                    _aiTypeMap = new Dictionary<int, Type>();
                    var abTypes = AppDomain.CurrentDomain.FindAllTypes((type) => type.IsSubclassOf(typeof(AIEntity)));
                    var errInfo = new StringBuilder();
                    foreach (var t in abTypes)
                    {
                        var err = AddAIType(t, _aiTypeMap);
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
                return _aiTypeMap;
            }
        }

        #endregion

        public static AIEntity Creat(int tmplId , Unit unit)
        {
            Type findType;
            if (AITypeMap.TryGetValue(tmplId, out findType))
            {
                var ai = Activator.CreateInstance(findType , unit) as AIEntity;
                return ai;
            }
            return null;
        }
    }
}
