namespace FASS.Extend.Door
{
    public static class Utility
    {
        public static string ByteArrayToHexString(byte[]? byteArray, string separator = "")
        {
            if (byteArray is null)
            {
                return string.Empty;
            }
            return string.Join(separator, byteArray.Select(t => t.ToString("X2")));
        }

        public static byte XOR(byte[] byteArray)
        {
            byte xor = 0;
            for (int i = 0; i < byteArray.Length; i++)
            {
                xor ^= byteArray[i];
            }
            return xor;
        }

        public static byte CheckSum(byte[] data)
        {
            byte sum = 0;
            for (int i = 0; i < (data.Length - 1); i++)
            {
                sum += data[i];
            }
            return sum;
        }

        public static bool[] SplitUint16ToBoolArray(ushort value)
        {
            bool[] boolArray = new bool[16];
            for (int i = 0; i < 16; i++)
            {
                boolArray[i] = ((value >> i) & 1) == 1;
            }
            return boolArray;
        }
    }
}
