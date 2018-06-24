namespace mana.Foundation
{
    public interface ISerializable
    {
        void Encode(IWritableBuffer bw);

        void Decode(IReadableBuffer br);
    }
}
