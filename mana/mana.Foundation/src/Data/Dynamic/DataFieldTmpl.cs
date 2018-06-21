namespace mana.Foundation
{
    public struct DataFieldTmpl
    {
        public static readonly DataFieldTmpl Empty = new DataFieldTmpl(DataToken.ft_none, null, false, null);

        public static DataFieldTmpl Decode(IReadableBuffer br)
        {
            var token   = (DataToken)br.ReadByte();
            var name    = br.ReadUTF8();
            var isArray = br.ReadBoolean();
            if (token == DataToken.ft_object)
            {
                var objTmpl = br.ReadUTF8();
                return new DataFieldTmpl(token, name, isArray, objTmpl);
            }
            else
            {
                return new DataFieldTmpl(token, name, isArray);
            }
        }

        public readonly DataToken token;
        public readonly string  name;
        public readonly bool    isArray;
        public readonly string  objTmpl;

        public DataFieldTmpl(DataToken _token, string _name, bool _isArray = false, string _objTmpl = null)
        {
            this.token      = _token;
            this.name       = _name;
            this.isArray    = _isArray;
            this.objTmpl    = _objTmpl;
        }

        public bool isUnknowType
        {
            get
            {
                return token == DataToken.ft_object && objTmpl == "DataObject";
            }
        }

        public override string ToString()
        {
            var sb = StringBuilderCache.Acquire();
            if (token == DataToken.ft_object)
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
            return StringBuilderCache.GetStringAndRelease(sb);
        }
    }
}
