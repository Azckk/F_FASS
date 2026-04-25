using Common.Service.Models;

namespace FASS.Service.Models.BaseExtend
{
    public class NodePlanRules : AuditModel
    {
        public required string NodeId { get; set; }
        public required string RuleName { get; set; }

    }
}
