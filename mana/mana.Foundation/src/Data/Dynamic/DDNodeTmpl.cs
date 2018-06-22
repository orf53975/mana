namespace mana.Foundation
{
    public sealed class DDNodeTmpl
    {
        #region <<Static Members>>
        public static DDNodeTmpl Decode(IReadableBuffer br)
        {
            var fname = br.ReadUTF8();
            var count = br.ReadByte();
            var fts = new DDFieldTmpl[count];
            for (var i = 0; i < count; i++)
            {
                fts[i] = DDFieldTmpl.Decode(br);
            }
            return new DDNodeTmpl(fname, fts);
        }
        #endregion

        public readonly DDFieldTmpl[] fieldTmpls;

        public readonly string fullName;

        public readonly string baseName;

        private DDNodeTmpl(string fn , DDFieldTmpl[] fts)
        {
            var idx = fn.LastIndexOf('.');
            if (idx >= 0)
            {
                this.baseName = fn.Substring(idx);
            }
            else
            {
                this.baseName = fn;
            }
            this.fullName = fn;
        }

        public int GetFieldIndex(string fieldName)
        {
            for (int i = 0; i < fieldTmpls.Length; i++)
            {
                if (fieldTmpls[i].name == fieldName)
                {
                    return i;
                }
            }
            return -1;
        }

        public DDFieldTmpl GetFieldTmpl(string fieldName)
        {
            for (int i = 0; i < fieldTmpls.Length; i++)
            {
                var ft = fieldTmpls[i];
                if (ft.name == fieldName)
                {
                    return ft;
                }
            }
            return DDFieldTmpl.Empty;
        }
    }
}