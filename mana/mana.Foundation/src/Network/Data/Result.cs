namespace mana.Foundation
{
    public sealed class Result : DataObject
    {
        public enum Code : int
        {
            unknow = 0,
            sucess = 1,
            falied = 2
        }

        public Code code = 0;

        public string info = null;


        public void Decode(IReadableBuffer br)
        {
            code = (Code)br.ReadInt();
            info = br.ReadUTF8();
        }

        public void Encode(IWritableBuffer bw)
        {
            bw.WriteInt((int)code);
            bw.WriteUTF8(info);
        }

        public void ReleaseToCache()
        {
            ObjectCache.Put(this);
        }

        public string ToFormatString(string nlIndent)
        {
            return string.Format("Result:code={0},info={1}", code, info == null ? "nulll" : info);
        }

        public override string ToString()
        {
            return this.ToFormatString(null);
        }

    }
}
