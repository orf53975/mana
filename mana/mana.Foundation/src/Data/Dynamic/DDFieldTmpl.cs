namespace mana.Foundation
{
    public struct DDFieldTmpl
    {
        #region <<Static Members>>
        public static readonly DDFieldTmpl Empty = new DDFieldTmpl(DDToken.ft_none, null, false, null);

        public static DDFieldTmpl Decode(IReadableBuffer br)
        {
            var token = (DDToken)br.ReadByte();
            var name = br.ReadUTF8();
            var isArray = br.ReadBoolean();
            if (token == DDToken.ft_object)
            {
                var objTmpl = br.ReadUTF8();
                return new DDFieldTmpl(token, name, isArray, objTmpl);
            }
            else
            {
                return new DDFieldTmpl(token, name, isArray);
            }
        }
        #endregion

        public readonly DDToken token;
        public readonly string name;
        public readonly bool isArray;
        public readonly string objTmpl;

        public DDFieldTmpl(DDToken _token, string _name, bool _isArray = false, string _objTmpl = null)
        {
            this.token = _token;
            this.name = _name;
            this.isArray = _isArray;
            this.objTmpl = _objTmpl;
        }

        public bool isUnknowType
        {
            get
            {
                return token == DDToken.ft_object && objTmpl == "DataObject";
            }
        }

        public override string ToString()
        {
            var sb = StringBuilderCache.Acquire();
            if (token == DDToken.ft_object)
            {
                sb.Append(objTmpl);
            }
            else
            {
                sb.Append(token);
            }
            if (isArray)
            {
                sb.Append("[]");
            }
            return StringBuilderCache.GetAndRelease(sb);
        }
    }
}