namespace FASS.Extend.Car.Fairyland.Pcb
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
    }
}
