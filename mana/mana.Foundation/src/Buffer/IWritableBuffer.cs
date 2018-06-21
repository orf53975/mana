using System;

namespace mana.Foundation
{
    public interface IWritableBuffer
    {
        void WriteByte(byte val);

        void WriteInt16(int val);

        void WriteInt24(int val);

        void WriteInt32(int val);

        void WriteInt64(long val);

        void Write(byte[] bytes, int offset, int count);

        void Write<T>(int count, T param, Action<byte[], int, int, T> dataSetter);

        void Write(int count, Action<byte[], int, int> dataSetter);
    }
}
