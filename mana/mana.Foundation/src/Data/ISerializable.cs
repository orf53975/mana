namespace mana.Foundation
{
    public interface ISerializable
    {
        void Encode(IWritableBuffer bw, bool bMaskAll);

        void Encode(IWritableBuffer bw);

        void Decode(IReadableBuffer br);
    }
}
