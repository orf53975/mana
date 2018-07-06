using System;
using System.Net.Sockets;

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

        public int PushData(Socket socket)
        {
            if (socket.Available <= 0)
            {
                return 0;
            }
            var needSpace = 1024;
            var newCapacity = buff.Length + needSpace;
            buff.EnsureCapacity(newCapacity);

            var buffer = buff.data;
            var offset = buff.Length;
            var size = needSpace;

            if(offset < 0 || offset + size > buffer.Length)
            {
                Logger.Error("aaaaaaaaaaaaaaaaaaaaaaaa");
            }
            var count = socket.Receive(buffer, offset, size, SocketFlags.None);
            if (count > 0)
            {
                buff.Write(count, null);
            }
            return count;
        }

        [Obsolete]
        private int PushData<T>(T param, Func<T, byte[], int, int, int> data_provider)
        {
            var needSpace = 1024;
            var newCapacity = buff.Length + needSpace;
            buff.EnsureCapacity(newCapacity);
            var count = data_provider.Invoke(param, buff.data, buff.Length, needSpace);
            buff.Write(count, null);
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