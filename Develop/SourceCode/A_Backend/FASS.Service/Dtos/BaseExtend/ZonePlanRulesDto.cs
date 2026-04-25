using Common.Service.Dtos;

namespace FASS.Service.Dtos.BaseExtend
{
    public class ZonePlanRulesDto : AuditDto
    {
        public required string ZoneId { get; set; }
        public required string RuleName { get; set; }
    }
}
