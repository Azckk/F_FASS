using Common.Service.Entities;

namespace FASS.Service.Entities.FlowExtend
{
    public class TaskTemplateRuleEntity : AuditEntity
    {
        public required string TaskTemplateId { get; set; }

        public required string Code { get; set; }
        public string? Description { get; set; }
        public required string Value { get; set; }

        public virtual TaskTemplateMdcsEntity TaskTemplateMdcs { get; set; } = null!;

    }
}
