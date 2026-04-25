namespace FASS.Extend.Charge.Pcb.Model
{
    public class Charger
    {
        public int SiteId { get; set; }
        public required string Ip { get; set; }
        public bool LinkState { get; set; }
        public ChargeState ChargeState { get; set; }
        public ElectrodeState ElectrodeState { get; set; }
        public int ChargeTime { get; set; }
        public double ChargeVoltage { get; set; }
        public double ChargeElectricity { get; set; }
        public double ChargingCapacity { get; set; }

        /// <summary>
        /// 侧充状态
        /// </summary>

        public DateTime LastRecvTime { get; set; }
    }

    /// <summary>
    /// 充电状态
    /// </summary>
    public enum ChargeState
    {
        /// <summary>
        /// 空闲中
        /// </summary>
        Idle = 0,
        /// <summary>
        /// 充电中
        /// </summary>
        Charging = 1,
        /// <summary>
        /// 报警中
        /// </summary>
        Alarming = 2,
        /// <summary>
        /// AGV电池已接入
        /// </summary>
        BatteryConnected = 3,
        /// <summary>
        /// 未知
        /// </summary>
        None = 4
    }

    /// <summary>
    /// 电极状态
    /// </summary>
    public enum ElectrodeState
    {
        /// <summary>
        /// 未知
        /// </summary>
        None = 0,
        /// <summary>
        /// 缩回
        /// </summary>
        Retracted = 1,
        /// <summary>
        /// 伸出
        /// </summary>
        Extended = 2,
        /// <summary>
        /// 运动中
        /// </summary>
        Runing = 3

    }
}
