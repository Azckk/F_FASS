namespace FASS.Extend.Call
{
    public class SendMessage
    {
        public byte Begin { get; set; }
        public byte Length { get; set; }
        public byte ButtonNo { get; set; }

        public SendMessage SetMessage(byte buttonNo)
        {
            Begin = 0x1A;
            Length = 0x01;
            ButtonNo = buttonNo;
            return this;
        }

        public SendMessage? GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length != 3)
            {
                return null;
            }
            Begin = byteArray[0];
            Length = byteArray[1];
            ButtonNo = byteArray[2];
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[3];
            byteArray[0] = Begin;
            byteArray[1] = Length;
            byteArray[2] = ButtonNo;
            return byteArray;
        }
    }
}
