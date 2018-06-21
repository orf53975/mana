using System;

namespace mana.Foundation
{
    public static class BufferExtension
    {
        #region <<IReadableBuffer Extension>>

        public static bool ReadBoolean(this IReadableBuffer br)
        {
            return br.ReadByte() != 0;
        }

        public static short ReadShort(this IReadableBuffer br)
        {
            return (short)br.ReadInt16();
        }

        public static ushort ReadUnsignedShort(this IReadableBuffer br)
        {
            return (ushort)br.ReadInt16();
        }

        public static int ReadInt(this IReadableBuffer br)
        {
            return br.ReadInt32();
        }

        public static long ReadLong(this IReadableBuffer br)
        {
            return br.ReadInt64();
        }

        public static float ReadFloat(this IReadableBuffer br)
        {
            var value = br.ReadInt32();
            return CodingUtil.DecodeFloat32(value);
        }

        public static float ReadFloat16(this IReadableBuffer br)
        {
            var v = (short)br.ReadInt16();
            return CodingUtil.DecodeFloat16(v);
        }

        public static void Read(this IReadableBuffer br, byte[] dat)
        {
            if (dat != null && dat.Length > 0)
            {
                br.Read(dat, 0, dat.Length);
            }
        }

        public static byte[] ReadBytes(this IReadableBuffer br, int count)
        {
            if (count < 0)
            {
                return null;
            }
            var buff = new byte[count];
            br.Read(buff, 0, count);
            return buff;
        }

        public static byte[] ReadAllBytes(this IReadableBuffer br)
        {

            var buff = new byte[br.Available];
            br.Read(buff, 0, buff.Length);
            return buff;
        }

        public static string ReadUTF8(this IReadableBuffer br)
        {
            var dat_len = br.ReadUnsignedShort();
            if (dat_len > 0)
            {
                using (var temp = ByteBuffer.Pool.Get())
                {
                    temp.Write(dat_len, br, (data, offset, len, param) => param.Read(data, offset, len));
                    return CodingUtil.EncodeUTF8(temp.data, 0, dat_len);
                }
            }
            return null;
        }

        public static long ReadUnsignedVarint(this IReadableBuffer br)
        {
            long value = 0;
            long b;
            byte bpos = 0;
            while (true)
            {
                b = br.ReadByte();
                value |= (b & 0x7F) << bpos;
                bpos += 7;
                if ((b & 0x80) == 0)
                {
                    break;
                }
            }
            return value;
        }

        public static long ReadVarint(this IReadableBuffer br)
        {
            long b = br.ReadByte();
            bool sign = (b & 0x40) != 0;
            bool next = (b & 0x80) != 0;
            long value = b & 0x3F;
            byte bpos = 6;
            while (next)
            {
                b = br.ReadByte();
                value |= (b & 0x7F) << bpos;
                bpos += 7;
                next = (b & 0x80) != 0;
            }
            if (sign)
            {
                value = ~value;
            }
            return value;
        }


        public static bool[] ReadBoolArray(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new bool[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadBoolean();
            }
            return ret;
        }

        public static byte[] ReadByteArray(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new byte[len];
            br.Read(ret);
            return ret;
        }

        public static short[] ReadShortArray(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new short[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadShort();
            }
            return ret;
        }

        public static int[] ReadIntArray(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new int[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadInt();
            }
            return ret;
        }

        public static float[] ReadFloatArray(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new float[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadFloat();
            }
            return ret;
        }

        public static float[] ReadFloat16Array(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new float[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadFloat16();
            }
            return ret;
        }

        public static long[] ReadLongArray(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new long[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadLong();
            }
            return ret;
        }

        public static string[] ReadUTF8Array(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new string[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadUTF8();
            }
            return ret;
        }

        public static long[] ReadVarintArray(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new long[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadVarint();
            }
            return ret;
        }

        public static long[] ReadUnsignedVarintArray(this IReadableBuffer br)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new long[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadUnsignedVarint();
            }
            return ret;
        }

        #endregion

        #region <<IWritableBuffer Extension>>

        public static void WriteBoolean(this IWritableBuffer bw, bool val)
        {
            if (val)
            {
                bw.WriteByte(1);
            }
            else
            {
                bw.WriteByte(0);
            }
        }

        public static void WriteShort(this IWritableBuffer bw, int val)
        {
            bw.WriteInt16(val);
        }

        public static void WriteUnsignedShort(this IWritableBuffer bw, int val)
        {
            bw.WriteInt16(val);
        }

        public static void WriteInt(this IWritableBuffer bw, int val)
        {
            bw.WriteInt32(val);
        }

        public static void WriteLong(this IWritableBuffer bw, long val)
        {
            bw.WriteInt64(val);
        }

        public static void WriteFloat(this IWritableBuffer bw, float val)
        {
            var i_v = CodingUtil.EncodeFloat32(val);
            bw.WriteInt(i_v);
        }

        public static void WriteFloat16(this IWritableBuffer bw, float val)
        {
            var i_v = CodingUtil.EncodeFloat16(val);
            bw.WriteShort(i_v);
        }

        public static void WriteUnsignedVarint(this IWritableBuffer bw, long mv)
        {
            long value = mv;
            byte b;
            while (true)
            {
                b = (byte)(value & 0x7F);
                value = (value >> 7) & 0x1FFFFFFFFFFFFFFL;
                if (value != 0)
                {
                    b |= 0x80;
                    bw.WriteByte(b);
                }
                else
                {
                    bw.WriteByte(b);
                    break;
                }
            }
        }

        public static void WriteVarint(this IWritableBuffer bw, long val)
        {
            byte b;
            if (val < 0)
            {
                val = ~val;
                b = (byte)(val & 0x3F);
                b |= 0x40;
            }
            else
            {
                b = (byte)(val & 0x3F);
            }
            val = val >> 6;
            if (val != 0)
            {
                b |= 0x80;
            }
            bw.WriteByte(b);
            while (val != 0)
            {
                b = (byte)(val & 0x7F);
                val = val >> 7;
                if (val != 0)
                {
                    b |= 0x80;
                    bw.WriteByte(b);
                }
                else
                {
                    bw.WriteByte(b);
                    break;
                }
            }
        }

        public static void Write(this IWritableBuffer bw, byte[] bytes)
        {
            bw.Write(bytes, 0, bytes.Length);
        }

        public static void Write(this IWritableBuffer bw, ByteBuffer other)
        {
            if (other == null || other.Length == 0)
            {
                return;
            }
            if (other == bw)
            {
                throw new ArgumentException();
            }
            bw.Write(other.data, 0, other.Length);
        }

        public static void WriteUTF8(this IWritableBuffer bw, string str)
        {
            if (str != null)
            {
                using (var temp = ByteBuffer.Pool.Get())
                {
                    CodingUtil.DecodeUTF8(str, temp);
                    bw.WriteUnsignedShort(temp.Length);
                    bw.Write(temp.data, 0, temp.Length);
                }
            }
            else
            {
                bw.WriteShort(0);
            }
        }

        public static void WriteByteArray(this IWritableBuffer bw, byte[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            if (len > 0)
            {
                bw.Write(dat);
            }
        }

        public static void WriteBooleanArray(this IWritableBuffer bw, bool[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            for (var i = 0; i < len; i++)
            {
                bw.WriteBoolean(dat[i]);
            }
        }

        public static void WriteShortArray(this IWritableBuffer bw, short[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            for (var i = 0; i < len; i++)
            {
                bw.WriteShort(dat[i]);
            }
        }

        public static void WriteIntArray(this IWritableBuffer bw, int[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            for (var i = 0; i < len; i++)
            {
                bw.WriteInt(dat[i]);
            }
        }

        public static void WriteFloatArray(this IWritableBuffer bw, float[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            for (var i = 0; i < len; i++)
            {
                bw.WriteFloat(dat[i]);
            }
        }

        public static void WriteFloat16Array(this IWritableBuffer bw, float[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            for (var i = 0; i < len; i++)
            {
                bw.WriteFloat16(dat[i]);
            }
        }

        public static void WriteLongArray(this IWritableBuffer bw, long[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            for (var i = 0; i < len; i++)
            {
                bw.WriteLong(dat[i]);
            }
        }

        public static void WriteUTF8Array(this IWritableBuffer bw, string[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            for (var i = 0; i < len; i++)
            {
                bw.WriteUTF8(dat[i]);
            }
        }

        public static void WriteVarintArray(this IWritableBuffer bw, long[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            for (var i = 0; i < len; i++)
            {
                bw.WriteVarint(dat[i]);
            }
        }

        public static void WriteUnsignedVarintArray(this IWritableBuffer bw, long[] dat)
        {
            var len = dat == null ? 0 : dat.Length;
            bw.WriteUnsignedVarint(len);
            for (var i = 0; i < len; i++)
            {
                bw.WriteUnsignedVarint(dat[i]);
            }
        }
        #endregion

    }
}
