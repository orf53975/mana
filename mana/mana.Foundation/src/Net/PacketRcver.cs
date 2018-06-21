namespace mana.Foundation
{
    public sealed class PacketRcver
    {
        readonly ByteBuffer buffer = new ByteBuffer(2024);

        public PacketRcver() { }

        public void PushData(byte[] data, int offset, int len)
        {
            buffer.Write(data, offset, len);
        }

        public Packet Build()
        {
            var p = Packet.TryDecode(buffer);
            if (p != null)
            {
                buffer.DiscardReadBytes();
                return p;
            }
            else
            {
                return null;
            }
        }

        public void Clear()
        {
            buffer.Clear();
        }
    }
}