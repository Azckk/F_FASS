using Common.Service.Entities;

namespace FASS.Service.Entities.FlowExtend
{
    public class TaskTemplateMdcsEntity : AuditEntity
    {
        public required string CarTypeId { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double Priority { get; set; }

        public virtual ICollection<TaskTemplateRuleEntity> TaskTemplateRules { get; set; } = [];

        public virtual ICollection<TaskRecordEntity> TaskRecords { get; set; } = [];

    }
}
