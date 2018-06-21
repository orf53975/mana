namespace mana.Foundation
{
    public sealed class PacketSnder
    {
        private readonly ByteBuffer sendingData = new ByteBuffer(1024);

        public void WriteTo(byte[] buffer, ref int offset, int sendBufferLimit)
        {
            lock (sendingData)
            {
                var nBytes = sendBufferLimit - offset;
                if (nBytes > sendingData.Available)
                {
                    nBytes = sendingData.Available;
                }
                if (nBytes > 0)
                {
                    sendingData.Read(buffer, offset, nBytes);
                    offset += nBytes;
                    sendingData.DiscardReadBytes();
                }
            }
        }

        public void Push(Packet p)
        {
            lock (sendingData) { Packet.Encode(p, sendingData); }
        }
    }
}
