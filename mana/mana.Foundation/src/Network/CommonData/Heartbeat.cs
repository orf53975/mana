namespace mana.Foundation
{
    public sealed class Heartbeat : DataObject
    {
        public int timestamp = 0;

        public void Decode(IReadableBuffer br)
        {
            timestamp = br.ReadInt();
        }

        public void Encode(IWritableBuffer bw)
        {
            bw.WriteInt(timestamp);
        }

        public void ReleaseToCache()
        {
            ObjectCache.Put(this);
        }

        public string ToFormatString(string nlIndent)
        {
            return "Heartbeat:" + timestamp;
        }
    }
}
