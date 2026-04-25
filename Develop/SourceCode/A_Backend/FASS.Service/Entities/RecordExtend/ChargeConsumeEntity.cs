using Common.Service.Entities;

namespace FASS.Service.Entities.RecordExtend
{
    public class ChargeConsumeEntity : AuditEntity
    {
        public required string CarCode { get; set; }
        public float LastDN { get; set; }
        public float CurrentDN { get; set; }
        public float ConsumeDN { get; set; }
    }
}
