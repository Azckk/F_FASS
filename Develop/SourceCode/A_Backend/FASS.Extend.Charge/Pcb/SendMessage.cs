namespace FASS.Extend.Charge.Pcb
{
    public class SendMessage
    {
        /// <summary>
        /// 起始码
        /// </summary>
        public byte Begin { get; set; } = 0xBB;
        // <summary>
        /// 索引号
        /// </summary>
        public int ChargeIndex { get; set; }
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
        /// <summary>
        /// 每台充电桩索引自增关系
        /// </summary>
        private Dictionary<int, int> indexNoDic = new Dictionary<int, int>();

        public SendMessage SetMessage(
            byte command,
            ushort car,
            float chargeElectric,
            float chargeVoltage,
            ushort chargeTimeSpan,
            ushort soc,
            float electricCurrent,
            float voltage, int chargeIndex)
        {
            Command = command;
            ChargeIndex = chargeIndex;
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
            ChargeElectric = BitConverter.ToSingle(new byte[] { byteArray[6], byteArray[5], byteArray[4], byteArray[3] }, 0);
            ChargeElectric = BitConverter.ToSingle(new byte[] { byteArray[10], byteArray[9], byteArray[8], byteArray[7] }, 0);
            ChargeTimeSpan = BitConverter.ToUInt16(new byte[] { byteArray[12], byteArray[11] }, 0);
            Car = BitConverter.ToUInt16(new byte[] { byteArray[14], byteArray[13] }, 0);
            Soc = byteArray[15];
            ElectricCurrent = BitConverter.ToSingle(new byte[] { byteArray[20], byteArray[19], byteArray[18], byteArray[17] }, 0);
            Voltage = BitConverter.ToSingle(new byte[] { byteArray[24], byteArray[23], byteArray[22], byteArray[21] }, 0);
            Reserve = byteArray[25..31];
            End = byteArray[31];
            return this;
        }

        public byte[] GetByteArray()
        {
            var indexNo = (byte)GetIndexNO(ChargeIndex);
            var byteArray = new byte[32];
            byteArray[0] = Begin;
            byteArray[1] = indexNo;
            byteArray[2] = Command;
            var chargeElectric = BitConverter.GetBytes(ChargeElectric);
            byteArray[3] = chargeElectric[3];
            byteArray[4] = chargeElectric[2];
            byteArray[5] = chargeElectric[1];
            byteArray[6] = chargeElectric[0];
            var chargeVoltage = BitConverter.GetBytes(ChargeVoltage);
            byteArray[7] = chargeVoltage[3];
            byteArray[8] = chargeVoltage[2];
            byteArray[9] = chargeVoltage[1];
            byteArray[10] = chargeVoltage[0];
            var chargeTimeSpan = BitConverter.GetBytes(ChargeTimeSpan);
            byteArray[11] = chargeTimeSpan[1];
            byteArray[12] = chargeTimeSpan[0];
            var car = BitConverter.GetBytes(Car);
            byteArray[13] = car[1];
            byteArray[14] = car[0];
            var soc = BitConverter.GetBytes(Soc);
            // byteArray[15] = soc[1];
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
            var crcCode = GetCRC(byteArray.Skip(1).Take(28).ToArray()).Reverse().ToArray();
            byteArray[29] = crcCode[0];
            byteArray[30] = crcCode[1];

            return byteArray;
        }

        private int GetIndexNO(int chargeIndex)
        {
            var no = 0;
            if (indexNoDic.ContainsKey(chargeIndex))
            {
                no = indexNoDic[chargeIndex];
                if (no < 255)
                {
                    indexNoDic[chargeIndex] = no + 1;
                }
                else
                {
                    indexNoDic[chargeIndex] = 0;
                }
            }
            else
            {
                indexNoDic[chargeIndex] = 1;
            }
            return no;
        }
        private byte[] GetCRC(byte[] data)
        {
            byte b = byte.MaxValue;
            byte b2 = byte.MaxValue;
            byte b3 = 1;
            byte b4 = 160;
            for (int i = 0; i < data.Length; i++)
            {
                b = (byte)(b ^ data[i]);
                for (int j = 0; j <= 7; j++)
                {
                    byte b5 = b2;
                    byte b6 = b;
                    b2 = (byte)(b2 >> 1);
                    b = (byte)(b >> 1);
                    if ((b5 & 1) == 1)
                    {
                        b = (byte)(b | 0x80u);
                    }
                    if ((b6 & 1) == 1)
                    {
                        b2 = (byte)(b2 ^ b4);
                        b = (byte)(b ^ b3);
                    }
                }
            }
            return new byte[2]
            {
            b,b2
            };
        }



    }
}
