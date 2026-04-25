

namespace FASS.Service.Models.Set
{
    public class ConfigChargeSet
    {
        public string? CarMinBattery { get; set; }
        public string? CarMaxBattery { get; set; }
        public string? CarIdleSecond { get; set; }
        public string? CarIdleChargeBattery { get; set; }
        public string? TaskAvailableBattery { get; set; }
        public bool IsChargingDuringIdleTime { get; set; }
      
    }

}
