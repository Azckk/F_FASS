using Common.Service.Entities;

namespace FASS.Service.Entities.BaseExtend
{
    public class ZonePlanRulesEntity : AuditEntity
    {
        public required string ZoneId { get; set; }
        public required string RuleName { get; set; }

    }
}
