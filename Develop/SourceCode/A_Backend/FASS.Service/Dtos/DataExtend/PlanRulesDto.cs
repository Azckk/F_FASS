using Common.Service.Dtos;

namespace FASS.Service.Dtos.DataExtend
{
    public class PlanRulesDto : AuditDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
        public string? NodeId { get; set; }

    }
}
