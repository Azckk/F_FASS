namespace FASS.Extend.Charge.Pcb
{
    public class ReceiveMessage
    {
        /// <summary>
        /// 起始码
        /// </summary>
        public byte Begin { get; set; } = 0xBB;

        // <summary>
        /// 索引号
        /// </summary>
        public int ChargeIndex { get; set; }

        // <summary>
        /// 充电站编号
        /// </summary>
        public int ChargeCode { get; set; }

        /// <summary>
        /// 实际充电电流
        /// </summary>
        public float CurrentElectric { get; set; }

        /// <summary>
        /// 实际充电电压
        /// </summary>
        public float CurrentVoltage { get; set; }

        /// <summary>
        /// 实际充电时长(s)
        /// </summary>
        public ushort ChargeTimeSpan { get; set; }

        /// <summary>
        /// 充电桩状态
        /// </summary>
        public byte ChargeState { get; set; }

        /// <summary>
        /// 累计充电量
        /// </summary>
        public float AccumulateCharging { get; set; }

        /// <summary>
        /// 侧充状态
        /// </summary>
        public byte ElectrodeState { get; set; }

        /// <summary>
        /// 校验位
        /// </summary>
        public byte Check { get; set; }

        /// <summary>
        /// 结束码
        /// </summary>
        public byte End { get; set; } = 0xEE;

        public ReceiveMessage SetMessage(
            float chargeElectric,
            float chargeVoltage,
            ushort chargeTimeSpan,
            byte chargeState,
            byte electrodeState
        )
        {
            CurrentElectric = chargeElectric;
            CurrentVoltage = chargeVoltage;
            ChargeTimeSpan = chargeTimeSpan;
            ChargeState = chargeState;
            ElectrodeState = electrodeState;
            return this;
        }

        public ReceiveMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 32)
                throw new Exception($"数据长度错误：{Utility.ByteArrayToHexString(byteArray)}");
            //if (byteArray[30] != Utility.CheckSum(byteArray[1..^2])) throw new Exception($"数据校验错误：{Utility.ByteArrayToHexString(byteArray)}");
            Begin = byteArray[0];
            ChargeIndex = byteArray[1];
            CurrentElectric = BitConverter.ToSingle(
                new byte[] { byteArray[5], byteArray[4], byteArray[3], byteArray[2] },
                0
            );
            CurrentVoltage = BitConverter.ToSingle(
                new byte[] { byteArray[9], byteArray[8], byteArray[7], byteArray[6] },
                0
            );
            ChargeTimeSpan = BitConverter.ToUInt16(new byte[] { byteArray[11], byteArray[10] }, 0);
            ChargeState = byteArray[14];
            ChargeCode = byteArray[15];
            AccumulateCharging = BitConverter.ToUInt16(
                new byte[] { byteArray[17], byteArray[16] },
                0
            );
            ElectrodeState = byteArray[28];
            Check = byteArray[30];
            End = byteArray[31];
            return this;
        }


    }
}
