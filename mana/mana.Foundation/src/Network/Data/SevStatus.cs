namespace mana.Foundation
{
    public sealed class SevStatus : DataObject
    {
        public float balance = 0.0f;

        public void Decode(IReadableBuffer br)
        {
            balance = br.ReadFloat();
        }

        public void Encode(IWritableBuffer bw)
        {
            bw.WriteFloat(balance);
        }

        public void ReleaseToCache()
        {
            ObjectCache.Put(this);
        }

        public string ToFormatString(string nlIndent)
        {
            return string.Format("ServerStatus: balance = {0}", balance);
        }
    }
}
