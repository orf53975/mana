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

        public readonly Mask mask = new Mask();

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
        public void Encode(IWritableBuffer bw, bool isMaskAll)
        {
            if (isMaskAll)
            {
                Mask.EncodeAllBit(bw, fields.Count);
            }
            else
            {
                mask.Encode(bw);
            }
            for (byte i = 0; i < fields.Count; i++)
            {
                if (isMaskAll || mask.CheckFlag(i))
                {
                    fields[i].Encode(bw, isMaskAll);
                }
            }
        }

        public void Encode(IWritableBuffer bw)
        {
            this.Encode(bw, false);
        }

        public void Decode(IReadableBuffer br)
        {
            this.mask.Decode(br);
            for (byte i = 0; i < fields.Count; i++)
            {
                if (mask.CheckFlag(i))
                {
                    fields[i].Decode(br);
                }
            }
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