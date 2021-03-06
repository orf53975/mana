﻿using System;

namespace mana.Foundation
{
    public sealed class Mask
    {
        #region <<Static Cache>>
        internal static readonly ObjectPool<Mask> Cache = new ObjectPool<Mask>(() => new Mask(), null, (e) => e.ClearAllFlag());
        #endregion

        private long value = 0;

        internal Mask(long value)
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
