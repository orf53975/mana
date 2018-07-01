namespace mana.Foundation
{
    public sealed class Response : DataObject
    {
        public enum Code : int
        {
            sucess = 0,
            falied = 1
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
            return "ComResponse:" + code;
        }
    }
}
