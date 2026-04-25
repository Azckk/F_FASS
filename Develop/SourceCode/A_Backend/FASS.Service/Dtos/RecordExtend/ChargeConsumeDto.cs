using Common.Service.Dtos;

namespace FASS.Service.Dtos.RecordExtend
{
    public class ChargeConsumeDto : AuditDto
    {
        public required string CarCode { get; set; }
        public required float LastDN { get; set; }
        public float CurrentDN { get; set; }
        public float ConsumeDN { get; set; }
    }
}
