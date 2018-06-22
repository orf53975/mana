using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public static class DDTmpl
    {
        static readonly Dictionary<string, DDNodeTmpl> nodeTmpls = new Dictionary<string, DDNodeTmpl>();

        public static DDNodeTmpl GetTmpl(string tmplName)
        {
            DDNodeTmpl ret;
            if (nodeTmpls.TryGetValue(tmplName, out ret))
            {
                return ret;
            }
            for (var iter = nodeTmpls.GetEnumerator(); iter.MoveNext();)
            {
                if (iter.Current.Value.baseName == tmplName)
                {
                    return iter.Current.Value;
                }
            }
            return null;
        }

        public static void Push(byte[] tmplData)
        {
            using (var br = ByteReader.Create(tmplData))
            {
                var count = br.ReadShort();
                for (int i = 0; i < count; i++)
                {
                    var tmpl = DDNodeTmpl.Decode(br);
                    nodeTmpls.Add(tmpl.fullName, tmpl);
                }
            }
        }

        public static List<DDNodeTmpl> GetAllTmpls()
        {
            return nodeTmpls.GetValues();
        }
    }
}