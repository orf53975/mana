using System.Collections.Generic;

namespace mana.Foundation
{
    public sealed class DDNode : ISerializable, ICacheable
    {
        public static DDNode Creat(string nodeTmpl)
        {
            var ret = ObjectCache.Get<DDNode>();
            ret.InitTmpl(nodeTmpl);
            return ret;
        }

        private List<DDField> fields = new List<DDField>(8);

        public DDNodeTmpl Tmpl
        {
            get;
            private set;
        }

        public void InitTmpl(string tmplName)
        {
            this.Tmpl = DDTmpl.GetTmpl(tmplName);
            this.fields.Clear();
            var fts = Tmpl.fieldTmpls;
            for (int i = 0; i < fts.Length; i++)
            {
                var field = ObjectCache.Get<DDField>();
                field.InitTmpl(fts[i]);
                fields.Add(field);
            }
        }

        public DDField this[string fieldName]
        {
            get
            {
                var fi = Tmpl.GetFieldIndex(fieldName);
                if (fi == -1)
                {
                    Logger.Error("can't find field[{0}] in [{1}]", fieldName, Tmpl.fullName);
                    return null;
                }
                return fields[fi];
            }
        }

        #region <<implement ISerializable>>
        public void Encode(IWritableBuffer bw)
        {
            var mask = Mask.Cache.Get();
            for (byte i = 0; i < fields.Count; i++)
            {
                if (fields[i].maskBit)
                {
                    mask.AddFlag(i);
                }
            }
            mask.Encode(bw);
            for (byte i = 0; i < fields.Count; i++)
            {
                if (mask.CheckFlag(i))
                {
                    fields[i].Encode(bw);
                }
            }
            Mask.Cache.Put(mask);
        }

        public void Decode(IReadableBuffer br)
        {
            var mask = Mask.Cache.Get();
            mask.Decode(br);
            for (byte i = 0; i < fields.Count; i++)
            {
                if (mask.CheckFlag(i))
                {
                    fields[i].Decode(br);
                }
            }
            Mask.Cache.Put(mask);
        }
        #endregion

        #region <<implement IFormatString>>
        public string ToFormatString(string nlIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("{\r\n");
            var curIndent = nlIndent + '\t';
            sb.Append(curIndent).Append("tmpl = \"").Append(Tmpl.fullName).Append('\"');
            for (int i = 0; i < fields.Count; i++)
            {
                sb.Append(",\r\n").Append(curIndent);
                sb.Append(fields[i].ToFormatString(nlIndent));
            }
            sb.Append("\r\n");
            sb.Append(nlIndent).Append('}');
            return StringBuilderCache.GetAndRelease(sb);
        }
        #endregion

        #region <<implement ICacheable>>
        public void ReleaseToCache()
        {
            for (var i = fields.Count - 1; i >= 0; i--)
            {
                fields[i].ReleaseToCache();
                fields.RemoveAt(i);
            }
            this.Tmpl = null;
            ObjectCache.Put(this);
        }
        #endregion
    }
}