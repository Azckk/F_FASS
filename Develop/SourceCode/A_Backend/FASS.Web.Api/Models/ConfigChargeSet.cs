
using Common.Service.Dtos;

namespace FASS.Web.Api.Models
{
    public class ConfigChargeSet : AuditDto
    {
        public required string CarMinBattery { get; set; }
        public required string CarMaxBattery { get; set; }
        public required string CarIdleSecond { get; set; }
        public required string CarIdleChargeBattery { get; set; }
        public required string TaskAvailableBattery { get; set; }
        public bool IsChargingDuringIdleTime { get; set; }
    }

}
