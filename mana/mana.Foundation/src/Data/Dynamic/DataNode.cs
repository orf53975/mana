using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public sealed class DataNode : DataObject , Cacheable
    {

        public static DataNode Creat(string nodeTmpl)
        {
            var ret = ObjectCache.Get<DataNode>();
            ret.InitTmpl(nodeTmpl);
            return ret;
        }

        private List<DataField> fields = new List<DataField>(8);

        public readonly Mask mask = new Mask();

        public DataNodeTmpl Tmpl
        {
            get;
            private set;
        }

        public void InitTmpl(string tmplName)
        {
            this.Tmpl = DataNodeTmpl.GetTmpl(tmplName);
            this.fields.Clear();
            var fts = Tmpl.fieldTmpls;
            for (int i = 0; i < fts.Length; i++)
            {
                var field = ObjectCache.Get<DataField>();
                field.InitTmpl(fts[i]);
                fields.Add(field);
            }
        }

        public DataField this[string fieldName]
        {
            get
            {
                var fi = Tmpl.GetFieldIndex(fieldName);
                if (fi == -1)
                {
                    Logger.Error("can't find field[{0}] in [{1}]", fieldName, Tmpl.name);
                    return null;
                }
                return fields[fi];
            }
        }

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

        public string ToFormatString(string nlIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("{\r\n");
            var curIndent = nlIndent + '\t';
            sb.Append(curIndent).Append("tmpl = \"").Append(Tmpl.name).Append('\"');
            for (int i = 0; i < fields.Count; i++)
            {
                sb.Append(",\r\n").Append(curIndent);
                sb.Append(fields[i].ToFormatString(nlIndent));
            }
            sb.Append("\r\n");
            sb.Append(nlIndent).Append('}');
            return StringBuilderCache.GetStringAndRelease(sb);
        }

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
    }
}