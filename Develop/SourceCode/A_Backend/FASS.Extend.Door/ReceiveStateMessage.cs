namespace FASS.Extend.Door
{
    public class ReceiveStateMessage
    {
        /// <summary>
        /// 总运行次数Hi
        /// </summary>
        public ushort TotalRunsHi { get; set; }

        /// <summary>
        /// 总运行次数Lo
        /// </summary>
        public ushort TotalRunsLo { get; set; }

        /// <summary>
        /// 维护后运行次数Hi
        /// </summary>
        public ushort MaintenanceRunsHi { get; set; }

        /// <summary>
        /// 维护后运行次数Lo
        /// </summary>
        public ushort MaintenanceRunsLo { get; set; }

        /// <summary>
        /// 系统状态
        /// </summary>
        public ushort SysState { get; set; }

        /// <summary>
        /// 远程锁定状态
        /// </summary>
        public byte LockState { get; set; }

        /// <summary>
        /// 当前运行状态
        /// </summary>
        public byte CurrentState { get; set; }

        /// <summary>
        /// 电机温度
        /// </summary>
        public byte MotorTemperature { get; set; }

        /// <summary>
        /// 当前门体高度
        /// </summary>
        public byte CurrentHeight { get; set; }

        /// <summary>
        /// 母线电压
        /// </summary>
        public ushort Voltage { get; set; }

        /// <summary>
        /// 相线电流
        /// </summary>
        public ushort ElectricCurrent { get; set; }

        /// <summary>
        /// 电机运行速度
        /// </summary>
        public ushort MotorSpeed { get; set; }

        /// <summary>
        /// 传感器状态
        /// </summary>
        public bool[] SensorState { get; set; } = [];

        /// <summary>
        /// 当前绝对值数值
        /// </summary>
        public ushort AbsoluteValue { get; set; }

        /// <summary>
        /// 当前HALL计算值
        /// </summary>
        public ushort HallTotal { get; set; }

        /// <summary>
        /// 最后一次运行时间
        /// </summary>
        public ushort LastTime { get; set; }

        public ReceiveStateMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 28) throw new Exception($"数据长度错误：{Utility.ByteArrayToHexString(byteArray)}");

            TotalRunsHi = BitConverter.ToUInt16(new byte[] { byteArray[1], byteArray[0] }, 0);
            TotalRunsLo = BitConverter.ToUInt16(new byte[] { byteArray[3], byteArray[2] }, 0);
            MaintenanceRunsHi = BitConverter.ToUInt16(new byte[] { byteArray[5], byteArray[4] }, 0);
            MaintenanceRunsLo = BitConverter.ToUInt16(new byte[] { byteArray[7], byteArray[6] }, 0);
            SysState = BitConverter.ToUInt16(new byte[] { byteArray[9], byteArray[8] }, 0);
            CurrentState = byteArray[10];
            LockState = byteArray[11];
            CurrentHeight = byteArray[12];
            MotorTemperature = byteArray[13];
            Voltage = BitConverter.ToUInt16(new byte[] { byteArray[15], byteArray[14] }, 0);
            ElectricCurrent = BitConverter.ToUInt16(new byte[] { byteArray[17], byteArray[16] }, 0);
            MotorSpeed = BitConverter.ToUInt16(new byte[] { byteArray[19], byteArray[18] }, 0);
            SensorState = Utility.SplitUint16ToBoolArray(BitConverter.ToUInt16(new byte[] { byteArray[21], byteArray[20] }, 0));
            AbsoluteValue = BitConverter.ToUInt16(new byte[] { byteArray[23], byteArray[22] }, 0);
            HallTotal = BitConverter.ToUInt16(new byte[] { byteArray[25], byteArray[24] }, 0);
            LastTime = BitConverter.ToUInt16(new byte[] { byteArray[27], byteArray[26] }, 0);
            return this;
        }

    }
}
