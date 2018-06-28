namespace mana.Foundation
{
    public static class BitFlag
    {
        public static byte AddByteFlag(byte value, byte bitIndex)
        {
            if(bitIndex < 8)
            {
                byte _flag = (byte)(1 << bitIndex);
                value |= _flag;
            }
            return value;
        }

        public static bool TestByteFlag(byte value, byte bitIndex)
        {
            if (bitIndex < 8)
            {
                var _flag = (1 << bitIndex);
                return (value & _flag) != 0;
            }
            return false;
        }
    }
}
