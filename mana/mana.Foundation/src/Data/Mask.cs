using System;

namespace mana.Foundation
{
    public sealed class Mask
    {
        #region <<static>>

        public static void EncodeValue(IWritableBuffer bw, long value)
        {
            bw.WriteUnsignedVarint(value);
        }

        public static void EncodeAllBit(IWritableBuffer bw, int count)
        {
            bw.WriteUnsignedVarint((1L << count) - 1);
        }

        #endregion

        private long value = 0;

        public Mask(long value)
        {
            this.value = value;
        }

        public Mask() : this(0)
        {
        }

        public bool Empty
        {
            get { return value == 0; }
        }

        public bool CheckFlag(byte flagIndex)
        {
            long bfv = 1L << flagIndex;
            return (value & bfv) != 0;
        }

        public bool CheckFlags(params byte[] flagIndex)
        {
            long bfv = 1L;
            for (byte i = 0; i < flagIndex.Length; i++)
            {
                bfv |= 1L << flagIndex[i];
            }
            return (value & bfv) != 0;
        }

        public void AddFlag(byte flagIndex)
        {
            long bfv = 1L << flagIndex;
            value |= bfv;
        }

        public void AddFlags(params byte[] flagIndex)
        {
            long bfv = 1L;
            for (byte i = 0; i < flagIndex.Length; i++)
            {
                bfv |= 1L << flagIndex[i];
            }
            value |= bfv;
        }

        public void ClearFlag(byte flagIndex)
        {
            long bfv = 1L << flagIndex;
            value &= ~bfv;
        }

        public void ClearFlags(params byte[] flagIndex)
        {
            long bfv = 1L;
            for (byte i = 0; i < flagIndex.Length; i++)
            {
                bfv |= 1L << flagIndex[i];
            }
            value &= ~bfv;
        }

        public void ClearAllFlag()
        {
            value = 0;
        }

        public void Encode(IWritableBuffer bw)
        {
            bw.WriteUnsignedVarint(value);
        }

        public void Decode(IReadableBuffer br)
        {
            value = br.ReadUnsignedVarint();
        }
    }
}
