namespace FASS.Extend.Call
{
    public class ReceiveMessage
    {
        public byte Begin { get; set; }
        public byte Length { get; set; }
        public byte ButtonNo { get; set; }
        public byte ButtonStatus { get; set; }
        public byte Power { get; set; }
        public byte RSSI { get; set; }
        public byte QOS { get; set; }

        public ReceiveMessage? GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length != 7)
            {
                return null;
            }
            Begin = byteArray[0];
            Length = byteArray[1];
            ButtonNo = byteArray[2];
            ButtonStatus = byteArray[3];
            Power = byteArray[4];
            RSSI = byteArray[5];
            QOS = byteArray[6];
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[7];
            byteArray[0] = Begin;
            byteArray[1] = Length;
            byteArray[2] = ButtonNo;
            byteArray[3] = ButtonStatus;
            byteArray[4] = Power;
            byteArray[5] = RSSI;
            byteArray[6] = QOS;
            return byteArray;
        }
    }
}