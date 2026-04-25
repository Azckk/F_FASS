using Common.Service.Models;

namespace FASS.Service.Models.RecordExtend
{
    public class ChargeConsume : AuditModel
    {

        public required string CarCode { get; set; }
        public float LastDN { get; set; }
        public float CurrentDN { get; set; }
        public float ConsumeDN { get; set; }
    }
}
