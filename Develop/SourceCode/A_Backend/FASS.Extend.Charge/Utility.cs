namespace FASS.Extend.Charge
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

    }
}
