using Common.Service.Dtos;

namespace FASS.Service.Dtos.BaseExtend
{
    public class NodePlanRulesDto : AuditDto
    {
        public required string NodeId { get; set; }
        public required string RuleName { get; set; }
    }
}
