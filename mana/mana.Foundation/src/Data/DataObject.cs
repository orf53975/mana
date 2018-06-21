namespace mana.Foundation
{
    public interface DataObject
    {
        void Encode(IWritableBuffer bw, bool isMaskAll);

        void Encode(IWritableBuffer bw);

        void Decode(IReadableBuffer br);

        string ToFormatString(string nlIndent);
    }
}
