using Common.Service.Dtos;

namespace FASS.Service.Dtos.FlowExtend
{
    public class TaskTemplateRuleDto : AuditDto
    {
        public required string TaskTemplateId { get; set; }

        public required string Code { get; set; }
        public string? Description { get; set; }
        public required string Value { get; set; }
        public string? TaskTemplateCode { get; set; }

    }
}
