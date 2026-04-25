namespace FASS.Extend.Car.Fairyland.Pcb
{
    public class SendStateResponseMessage
    {
        public byte Begin { get; set; } = 0xBB;
        public byte Command { get; set; } = 0x10;
        public ushort Car { get; set; }
        public ulong TimeStamp { get; set; }
        public byte[] Reserve { get; set; } = new byte[28];
        public byte Check { get; set; }
        public byte End { get; set; } = 0xEE;

        public SendStateResponseMessage SetMessage(
            byte command,
            ushort car,
            ulong param)
        {
            Command = command;
            Car = car;
            TimeStamp = param;
            return this;
        }

        public SendStateResponseMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 50) throw new Exception($"数据长度错误： {Utility.ByteArrayToHexString(byteArray)}");
            //if (byteArray[48] != Utility.XOR(byteArray[1..^2])) throw new Exception($"数据校验错误：{Utility.ByteArrayToHexString(byteArray)}");
            Begin = byteArray[0];
            Command = byteArray[1];
            Car = BitConverter.ToUInt16(byteArray[2..4]);
            TimeStamp = BitConverter.ToUInt64(byteArray[4..12]);
            Reserve = byteArray[20..48];
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
            var ticks = BitConverter.GetBytes(TimeStamp);
            byteArray[4] = ticks[0];
            byteArray[5] = ticks[1];
            byteArray[6] = ticks[2];
            byteArray[7] = ticks[3];
            byteArray[8] = ticks[4];
            byteArray[9] = ticks[5];
            byteArray[10] = ticks[6];
            byteArray[11] = ticks[7];
            var date = new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(TimeStamp);
            byteArray[12] = (byte)(date.Year - 1970);
            byteArray[13] = (byte)(date.Month);
            byteArray[14] = (byte)(date.Day);
            byteArray[15] = (byte)(date.Hour);
            byteArray[16] = (byte)(date.Minute);
            byteArray[17] = (byte)(date.Second);
            var millisecond = BitConverter.GetBytes((ushort)(date.Millisecond));
            byteArray[18] = millisecond[0];
            byteArray[19] = millisecond[1];
            Array.Copy(Reserve, 0, byteArray, 20, Reserve.Length);
            //byteArray[48] = Check;
            byteArray[48] = Utility.XOR(byteArray[1..^2]);
            byteArray[49] = End;
            return byteArray;
        }
    }
}
