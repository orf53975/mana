using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public sealed class DataNodeTmpl
    {
        #region <<static>>

        static readonly Dictionary<string, DataNodeTmpl> AllTmpls = new Dictionary<string, DataNodeTmpl>();

        public static void Push(byte[] tmplData)
        {
            using(var br = ByteReader.Create(tmplData))
            {
                var count = br.ReadShort();
                for (int i = 0; i < count; i++)
                {
                    var tmpl = new DataNodeTmpl(br);
                    AllTmpls.Add(tmpl.name, tmpl);
                }
            }
        }

        public static DataNodeTmpl GetTmpl(string tmplName)
        {
            DataNodeTmpl ret = null;
            if (!AllTmpls.TryGetValue(tmplName, out ret))
            {
                for (var iter = AllTmpls.GetEnumerator(); iter.MoveNext();)
                {
                    if (iter.Current.Value.baseName == tmplName)
                    {
                        return iter.Current.Value;
                    }
                }
            }
            return ret;
        }

        public static List<DataNodeTmpl> GetAllTmpls()
        {
            return AllTmpls.GetValues();
        }

        #endregion

        public readonly DataFieldTmpl[] fieldTmpls;

        public readonly string name;

        public readonly string baseName;

        private DataNodeTmpl(IReadableBuffer br)
        {
            this.name = br.ReadUTF8();
            var count = br.ReadShort();//TODO ReadByte
            this.fieldTmpls = new DataFieldTmpl[count];
            for (var i = 0; i < count; i++)
            {
                fieldTmpls[i] = DataFieldTmpl.Decode(br);
            }
            var idx = this.name.LastIndexOf('.');
            if (idx >= 0)
            {
                this.baseName = name.Substring(idx);
            }
            else
            {
                this.baseName = name;
            }
        }

        public int GetFieldIndex(string fieldName)
        {
            for(int i = 0; i < fieldTmpls.Length; i++)
            {
                if(fieldTmpls[i].name == fieldName)
                {
                    return i;
                }
            }
            return -1;
        }

        public DataFieldTmpl GetFieldTmpl(string fieldName)
        {
            for (int i = 0; i < fieldTmpls.Length; i++)
            {
                var ft = fieldTmpls[i];
                if (ft.name == fieldName)
                {
                    return ft;
                }
            }
            return DataFieldTmpl.Empty;
        }
    }
}