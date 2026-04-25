namespace FASS.Extend.Charge.Pcb.Model
{
    public class ChargeStation
    {
        public string? Id { get; set; }
        public string? ChargeId { get; set; }
        public required string ChargeCode { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Ip { get; set; }
        public int Port { get; set; }
        public ProtocolType Protocol { get; set; }
        public ChargeModel Mode { get; set; }
        public double Voltage { get; set; }
        public double Current { get; set; }
        public bool IsOccupied { get; set; }
        public string? OccupiedCarId { get; set; }
        public string? State { get; set; }
        public double SortNumber { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDelete { get; set; }
    }

    public enum ProtocolType
    {
        Tcp = 0,
        Udp = 1
    }

    public enum ChargeModel
    {
        SideCharging = 0,
        BoomCharging = 1
    }
}
