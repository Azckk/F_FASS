using Common.Service.Models;

namespace FASS.Service.Models.BaseExtend
{
    public class ZonePlanRules : AuditModel
    {
        public required string ZoneId { get; set; }
        public required string RuleName { get; set; }

    }
}
