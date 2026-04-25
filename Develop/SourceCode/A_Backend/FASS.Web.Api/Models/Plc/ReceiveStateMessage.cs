namespace FASS.Web.Api.Models.Plc
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
    }
}
