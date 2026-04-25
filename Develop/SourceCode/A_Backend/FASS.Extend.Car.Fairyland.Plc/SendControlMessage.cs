namespace FASS.Extend.Car.Fairyland.Plc
{
    public class SendControlMessage
    {
        public byte Begin { get; set; } = 0xBB;
        public byte Command { get; set; } = 0x00;
        public ushort Car { get; set; }
        public ushort Param { get; set; }
        public byte[] Reserve { get; set; } = new byte[41];
        public byte Check { get; set; }
        public byte End { get; set; } = 0xEE;

        public SendControlMessage SetMessage(
            byte command,
            ushort car,
            ushort param)
        {
            Command = command;
            Car = car;
            Param = param;
            return this;
        }

        public SendControlMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 50) throw new Exception($"数据长度错误： {Utility.ByteArrayToHexString(byteArray)}");
            //if (byteArray[48] != Utility.XOR(byteArray[1..^2])) throw new Exception($"数据校验错误：{Utility.ByteArrayToHexString(byteArray)}");
            Begin = byteArray[0];
            Command = byteArray[1];
            Car = BitConverter.ToUInt16(byteArray[2..4]);
            Param = BitConverter.ToUInt16(byteArray[4..6]);
            Reserve = byteArray[6..48];
            Check = byteArray[48];
            End = byteArray[49];
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[50];
            byteArray[0] = Begin;
            byteArray[1] = Command;
            var car = BitConverter.GetBytes(Car);
            byteArray[2] = car[0];
            byteArray[3] = car[1];
            var param = BitConverter.GetBytes(Param);
            byteArray[4] = param[0];
            byteArray[5] = param[1];
            Array.Copy(Reserve, 0, byteArray, 6, Reserve.Length);
            //byteArray[48] = Check;
            byteArray[48] = Utility.XOR(byteArray[1..^2]);
            byteArray[49] = End;
            return byteArray;
        }
    }
}