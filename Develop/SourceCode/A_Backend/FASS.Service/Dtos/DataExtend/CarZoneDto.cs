using Common.Service.Dtos;

namespace FASS.Service.Dtos.DataExtend
{
    public class CarZoneDto : AuditDto
    {
        public required string CarId { get; set; }
        public required string ZoneId { get; set; }
        public string? ZoneCode { get; set; }
        public string? ZoneName { get; set; }
    }
}
