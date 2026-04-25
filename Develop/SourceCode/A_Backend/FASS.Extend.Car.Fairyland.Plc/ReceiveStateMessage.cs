using System.Text;

namespace FASS.Extend.Car.Fairyland.Plc
{
    public class ReceiveStateMessage
    {
        public byte Begin { get; set; } = 0xBB;
        public byte Command { get; set; }
        public ushort Car { get; set; }
        public ushort Length { get; set; }
        public ushort Width { get; set; }
        public ushort SystemVersion { get; set; }
        public ushort SupportVersion { get; set; }
        public string? CarType { get; set; }
        public byte BatteryCharge { get; set; }
        public byte BatteryHealth { get; set; }
        public ushort BatteryCurrent { get; set; }
        public ushort BatteryVoltage { get; set; }
        public ushort HeadingAngle { get; set; }
        public byte State { get; set; }
        public ulong Alarm { get; set; }
        public ulong Task { get; set; }
        public NodeMessage NodeMessage { get; set; } = new NodeMessage();
        public byte[] Reserve { get; set; } = new byte[20];
        public byte Check { get; set; }
        public byte End { get; set; } = 0xEE;

        public ReceiveStateMessage SetMessage(
            byte command,
            ushort car,
            ushort length,
            ushort width,
            ushort systemVersion,
            ushort supportVersion,
            string carType,
            byte batteryCharge,
            byte batteryHealth,
            ushort batteryCurrent,
            ushort batteryVoltage,
            byte state,
            ulong alarm,
            ulong task,
            NodeMessage nodeMessage)
        {
            Command = command;
            Car = car;
            Length = length;
            Width = width;
            SystemVersion = systemVersion;
            SupportVersion = supportVersion;
            CarType = carType;
            BatteryCharge = batteryCharge;
            BatteryHealth = batteryHealth;
            BatteryCurrent = batteryCurrent;
            BatteryVoltage = batteryVoltage;
            State = state;
            Alarm = alarm;
            Task = task;
            NodeMessage = nodeMessage;
            return this;
        }

        public ReceiveStateMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 100) throw new Exception($"数据长度错误：{Utility.ByteArrayToHexString(byteArray)}");
            //if (byteArray[98] != Utility.XOR(byteArray[1..^2])) throw new Exception($"数据校验错误：{Utility.ByteArrayToHexString(byteArray)}");
            Begin = byteArray[0];
            Command = byteArray[1];
            Car = BitConverter.ToUInt16(byteArray[2..4]);
            Length = BitConverter.ToUInt16(byteArray[4..6]);
            Width = BitConverter.ToUInt16(byteArray[6..8]);
            SystemVersion = BitConverter.ToUInt16(byteArray[8..10]);
            SupportVersion = BitConverter.ToUInt16(byteArray[10..12]);
            CarType = Encoding.ASCII.GetString(byteArray[12..28]).PadLeft(16, '0');
            BatteryCharge = byteArray[28];
            BatteryHealth = byteArray[29];
            BatteryCurrent = BitConverter.ToUInt16(byteArray[30..32]);
            BatteryVoltage = BitConverter.ToUInt16(byteArray[32..34]);
            HeadingAngle = BitConverter.ToUInt16(byteArray[34..36]);
            State = byteArray[36];
            Alarm = BitConverter.ToUInt64(byteArray[37..45]);
            Task = BitConverter.ToUInt64(byteArray[45..53]);
            NodeMessage = new NodeMessage().GetMessage(byteArray[53..78]);
            Reserve = byteArray[78..98];
            Check = byteArray[98];
            End = byteArray[99];
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[100];
            byteArray[0] = Begin;
            byteArray[1] = Command;
            var car = BitConverter.GetBytes(Car);
            byteArray[2] = car[0];
            byteArray[3] = car[1];
            var length = BitConverter.GetBytes(Length);
            byteArray[4] = length[0];
            byteArray[5] = length[1];
            var width = BitConverter.GetBytes(Width);
            byteArray[6] = width[0];
            byteArray[7] = width[1];
            var systemVersion = BitConverter.GetBytes(SystemVersion);
            byteArray[8] = systemVersion[0];
            byteArray[9] = systemVersion[1];
            var supportVersion = BitConverter.GetBytes(SupportVersion);
            byteArray[10] = supportVersion[0];
            byteArray[11] = supportVersion[1];
            var carType = Encoding.ASCII.GetBytes((CarType ?? string.Empty).PadLeft(16, '0'));
            byteArray[12] = carType[0];
            byteArray[13] = carType[1];
            byteArray[14] = carType[2];
            byteArray[15] = carType[3];
            byteArray[16] = carType[4];
            byteArray[17] = carType[5];
            byteArray[18] = carType[6];
            byteArray[19] = carType[7];
            byteArray[20] = carType[8];
            byteArray[21] = carType[9];
            byteArray[22] = carType[10];
            byteArray[23] = carType[11];
            byteArray[24] = carType[12];
            byteArray[25] = carType[13];
            byteArray[26] = carType[14];
            byteArray[27] = carType[15];
            byteArray[28] = BatteryCharge;
            byteArray[29] = BatteryHealth;
            var batteryCurrent = BitConverter.GetBytes(BatteryCurrent);
            byteArray[30] = batteryCurrent[0];
            byteArray[31] = batteryCurrent[1];
            var batteryVoltage = BitConverter.GetBytes(BatteryVoltage);
            byteArray[32] = batteryVoltage[0];
            byteArray[33] = batteryVoltage[1];
            var headingAngle = BitConverter.GetBytes(HeadingAngle);
            byteArray[34] = headingAngle[0];
            byteArray[35] = headingAngle[1];
            byteArray[36] = State;
            var alarm = BitConverter.GetBytes(Alarm);
            byteArray[37] = alarm[0];
            byteArray[38] = alarm[1];
            byteArray[39] = alarm[2];
            byteArray[40] = alarm[3];
            byteArray[41] = alarm[4];
            byteArray[42] = alarm[5];
            byteArray[43] = alarm[6];
            byteArray[44] = alarm[7];
            var task = BitConverter.GetBytes(Task);
            byteArray[45] = task[0];
            byteArray[46] = task[1];
            byteArray[47] = task[2];
            byteArray[48] = task[3];
            byteArray[49] = task[4];
            byteArray[50] = task[5];
            byteArray[51] = task[6];
            byteArray[52] = task[7];
            var nodeMessage = NodeMessage.GetByteArray();
            Array.Copy(nodeMessage, 0, byteArray, 53, nodeMessage.Length);
            Array.Copy(Reserve, 0, byteArray, 78, Reserve.Length);
            //byteArray[98] = Check;
            byteArray[98] = Utility.XOR(byteArray[1..^2]);
            byteArray[99] = End;
            return byteArray;
        }
    }
}