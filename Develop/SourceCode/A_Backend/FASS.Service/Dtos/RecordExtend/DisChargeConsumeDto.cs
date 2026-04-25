using Common.Service.Dtos;

namespace FASS.Service.Dtos.RecordExtend
{
    /// <summary>
    /// 放电
    /// </summary>
    public class DisChargeConsumeDto : AuditDto
    {
        public required string CarCode { get; set; }
        public float LastDN { get; set; }
        public float CurrentDN { get; set; }
        public float ConsumeDN { get; set; }
    }
}
