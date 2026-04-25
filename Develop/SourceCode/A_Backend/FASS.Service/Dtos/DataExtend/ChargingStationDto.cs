using Common.Service.Dtos;

namespace FASS.Service.Dtos.DataExtend
{
    public class ChargingStationDto : AuditDto
    {
        public required string ChargeId { get; set; }
        public required string ChargeCode { get; set; }
        public  required string Code { get; set; }
        public string? Name { get; set; }
        public required string Ip { get; set; }
        public required int Port { get; set; }
        public required string Protocol { get; set; }
        public required string Mode { get; set; }
        public double Voltage { get; set; }
        public double Current { get; set; }
        public bool IsOccupied { get; set; }
        public string? OccupiedCarId { get; set; }
        public string? State { get; set; }
    }
}
