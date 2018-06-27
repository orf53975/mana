using System;

namespace mana.Foundation
{
    public sealed class PacketRcver
    {
        readonly ByteBuffer buff = new ByteBuffer(1024 + 256);

        public PacketRcver() { }

        public void PushData(byte[] data, int offset, int len)
        {
            buff.Write(data, offset, len);
        }

        public int PushData<T>(T param, Func<T, byte[], int, int, int> data_provider)
        {
            var needSpace = 1024;
            var newCapacity = buff.Length + needSpace;
            buff.EnsureCapacity(newCapacity);
            var count = data_provider.Invoke(param, buff.data, buff.Length, needSpace);
            buff.Write(count , null);
            return count;
        }

        public Packet Build()
        {
            var p = Packet.TryDecode(buff);
            if (p != null)
            {
                buff.DiscardReadBytes();
            }
            return p;
        }

        public void Clear()
        {
            buff.Clear();
        }
    }
}