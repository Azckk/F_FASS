namespace FASS.Extend.Charge.plc
{
    public class SendMessage
    {
        /// <summary>
        /// 起始码
        /// </summary>
        public byte Begin { get; set; } = 0xBB;

        /// <summary>
        /// 充电动作 0查询、1启动，2停止，3急停（预留功能）
        /// </summary>
        public byte Command { get; set; }

        /// <summary>
        /// 充电电流
        /// </summary>
        public float ChargeElectric { get; set; }

        /// <summary>
        /// 充电电压
        /// </summary>
        public float ChargeVoltage { get; set; }

        /// <summary>
        /// 充电时长
        /// </summary>
        public ushort ChargeTimeSpan { get; set; }

        /// <summary>
        /// 车辆编号
        /// </summary>
        public ushort Car { get; set; }

        /// <summary>
        /// 电池电量
        /// </summary>
        public ushort Soc { get; set; }

        /// <summary>
        /// 电池电流
        /// </summary>
        public float ElectricCurrent { get; set; }

        /// <summary>
        /// 电池电压
        /// </summary>
        public float Voltage { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public byte[] Reserve { get; set; } = new byte[7];
        public byte End { get; set; } = 0xEE;


        public SendMessage SetMessage(
            byte command,
            ushort car,
            float chargeElectric,
            float chargeVoltage,
            ushort chargeTimeSpan,
            ushort soc,
            float electricCurrent,
            float voltage)
        {
            Command = command;
            Car = car;
            ChargeElectric = chargeElectric;
            ChargeVoltage = chargeVoltage;
            ChargeTimeSpan = chargeTimeSpan;
            Soc = soc;
            ElectricCurrent = electricCurrent;
            Voltage = voltage;
            return this;
        }

        public SendMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 32) throw new Exception($"数据长度错误： {Utility.ByteArrayToHexString(byteArray)}");
            //if (byteArray[30] != Utility.CheckSum(byteArray[1..^2])) throw new Exception($"数据校验错误：{Utility.ByteArrayToHexString(byteArray)}");
            Begin = byteArray[0];
            Command = byteArray[1];
            ChargeElectric = BitConverter.ToSingle(new byte[] { byteArray[5], byteArray[4], byteArray[3], byteArray[2] }, 0);
            ChargeElectric = BitConverter.ToSingle(new byte[] { byteArray[9], byteArray[8], byteArray[7], byteArray[6] }, 0);
            ChargeTimeSpan = BitConverter.ToUInt16(new byte[] { byteArray[11], byteArray[10] }, 0);
            Car = BitConverter.ToUInt16(new byte[] { byteArray[13], byteArray[12] }, 0);
            Soc = BitConverter.ToUInt16(new byte[] { byteArray[15], byteArray[14] }, 0);
            ElectricCurrent = BitConverter.ToSingle(new byte[] { byteArray[19], byteArray[18], byteArray[17], byteArray[16] }, 0);
            Voltage = BitConverter.ToSingle(new byte[] { byteArray[23], byteArray[22], byteArray[21], byteArray[20] }, 0);
            Reserve = byteArray[24..31];
            End = byteArray[31];
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[32];
            byteArray[0] = Begin;
            byteArray[1] = Command;
            var chargeElectric = BitConverter.GetBytes(ChargeElectric);
            byteArray[2] = chargeElectric[3];
            byteArray[3] = chargeElectric[2];
            byteArray[4] = chargeElectric[1];
            byteArray[5] = chargeElectric[0];
            var chargeVoltage = BitConverter.GetBytes(ChargeVoltage);
            byteArray[6] = chargeVoltage[3];
            byteArray[7] = chargeVoltage[2];
            byteArray[8] = chargeVoltage[1];
            byteArray[9] = chargeVoltage[0];
            var chargeTimeSpan = BitConverter.GetBytes(ChargeTimeSpan);
            byteArray[10] = chargeTimeSpan[1];
            byteArray[11] = chargeTimeSpan[0];
            var car = BitConverter.GetBytes(Car);
            byteArray[12] = car[1];
            byteArray[13] = car[0];
            var soc = BitConverter.GetBytes(Soc);
            byteArray[14] = soc[1];
            byteArray[15] = soc[0];
            var electricCurrent = BitConverter.GetBytes(ElectricCurrent);
            byteArray[16] = electricCurrent[3];
            byteArray[17] = electricCurrent[2];
            byteArray[18] = electricCurrent[1];
            byteArray[19] = electricCurrent[0];
            var voltage = BitConverter.GetBytes(Voltage);
            byteArray[20] = voltage[3];
            byteArray[21] = voltage[2];
            byteArray[22] = voltage[1];
            byteArray[23] = voltage[0];
            Array.Copy(Reserve, 0, byteArray, 24, Reserve.Length);
            byteArray[31] = End;
            return byteArray;
        }

    }
}
