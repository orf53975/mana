namespace mana.Foundation
{
    public interface IReadableBuffer
    {
        int Available { get; }

        int Position { get; }

        byte ReadByte();

        int ReadInt16();

        int ReadInt24();

        int ReadInt32();

        long ReadInt64();

        void Read(byte[] buffer, int offset, int count);

        void Skip(int n);

        void Seek(int newPosition);
    }
}
