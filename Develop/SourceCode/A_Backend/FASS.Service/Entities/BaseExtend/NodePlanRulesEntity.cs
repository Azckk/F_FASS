using Common.Service.Entities;

namespace FASS.Service.Entities.BaseExtend
{
    public class NodePlanRulesEntity : AuditEntity
    {
        public required string NodeId { get; set; }
        public required string RuleName { get; set; }

    }
}
