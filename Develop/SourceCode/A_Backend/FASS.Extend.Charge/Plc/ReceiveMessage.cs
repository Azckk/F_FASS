namespace FASS.Extend.Charge.plc
{
    public class ReceiveMessage
    {
        /// <summary>
        /// 起始码
        /// </summary>
        public byte Begin { get; set; } = 0xBB;

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
        /// 异常状态
        /// </summary>
        public ushort ExceptionState { get; set; }

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
            ushort exceptionState,
            byte electrodeState)
        {
            CurrentElectric = chargeElectric;
            CurrentVoltage = chargeVoltage;
            ChargeTimeSpan = chargeTimeSpan;
            ChargeState = chargeState;
            ExceptionState = exceptionState;
            ElectrodeState = electrodeState;
            return this;
        }

        public ReceiveMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 32) throw new Exception($"数据长度错误：{Utility.ByteArrayToHexString(byteArray)}");
            //if (byteArray[30] != Utility.CheckSum(byteArray[1..^2])) throw new Exception($"数据校验错误：{Utility.ByteArrayToHexString(byteArray)}");
            Begin = byteArray[0];
            CurrentElectric = BitConverter.ToSingle(new byte[] { byteArray[4], byteArray[3], byteArray[2], byteArray[1] }, 0);
            CurrentVoltage = BitConverter.ToSingle(new byte[] { byteArray[8], byteArray[7], byteArray[6], byteArray[5] }, 0);
            ChargeTimeSpan = BitConverter.ToUInt16(new byte[] { byteArray[10], byteArray[9] }, 0);
            ChargeState = byteArray[14];
            ExceptionState = BitConverter.ToUInt16(new byte[] { byteArray[16], byteArray[15] }, 0);
            ElectrodeState = byteArray[28];
            Check = byteArray[30];
            End = byteArray[31];
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[32];
            byteArray[0] = Begin;
            var chargeElectric = BitConverter.GetBytes(CurrentElectric);
            byteArray[1] = chargeElectric[3];
            byteArray[2] = chargeElectric[2];
            byteArray[3] = chargeElectric[1];
            byteArray[4] = chargeElectric[0];
            var chargeVoltage = BitConverter.GetBytes(CurrentVoltage);
            byteArray[5] = chargeVoltage[3];
            byteArray[6] = chargeVoltage[2];
            byteArray[7] = chargeVoltage[1];
            byteArray[8] = chargeVoltage[0];
            var chargeTimeSpan = BitConverter.GetBytes(ChargeTimeSpan);
            byteArray[9] = chargeTimeSpan[1];
            byteArray[10] = chargeTimeSpan[0];
            byteArray[14] = ChargeState;
            var exception = BitConverter.GetBytes(ExceptionState);
            byteArray[15] = exception[1];
            byteArray[16] = exception[0];
            byteArray[28] = ElectrodeState;
            //byteArray[30] = Utility.CheckSum(byteArray[1..^2]);
            byteArray[31] = End;
            return byteArray;
        }
    }
}
