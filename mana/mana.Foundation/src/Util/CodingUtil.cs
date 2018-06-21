using System;
using System.Text;

namespace mana.Foundation
{
    public static class CodingUtil
    {

        #region <<utf - 8>>

        private static readonly Encoding utf8 = new UTF8Encoding(false);

        public static byte[] DecodeUTF8(string str)
        {
            return utf8.GetBytes(str);
        }

        public static void DecodeUTF8(string str, ByteBuffer ret)
        {
            int count = utf8.GetByteCount(str);
            ret.Write(count, str, (data, offset, len, param) =>
            {
                utf8.GetBytes(param, 0, param.Length, data, offset);
            });
        }

        public static int GetByteCountUTF8(string str)
        {
            return utf8.GetByteCount(str);
        }

        public static string EncodeUTF8(byte[] bytes)
        {
            return utf8.GetString(bytes);
        }

        public static string EncodeUTF8(byte[] bytes, int index, int count)
        {
            return utf8.GetString(bytes, index, count);
        }

        #endregion


        #region <<float32>>

        public unsafe static int EncodeFloat32(float fv)
        {
            //return BitConverter.ToInt32(BitConverter.GetBytes(fv), 0);
            return *(int*)(&fv);
        }

        public unsafe static float DecodeFloat32(int iv)
        {
            //return BitConverter.ToSingle(BitConverter.GetBytes(iv), 0);
            return *(float*)(&iv);
        }

        #endregion


        #region <<float16>>

        const float S_2_C = 200f;
        const float F_C_S = 1.0f / S_2_C;
        public static short EncodeFloat16(float v)
        {
            int value = (int)(v * S_2_C);
            if (value < short.MinValue || value > short.MaxValue)
            {
                throw new Exception("float16 convert out of bounds");
            }
            else
            {
                return (short)(v * S_2_C);
            }
        }

        public static float DecodeFloat16(short half)
        {
            return half * F_C_S;
        }

        #endregion
    }
}