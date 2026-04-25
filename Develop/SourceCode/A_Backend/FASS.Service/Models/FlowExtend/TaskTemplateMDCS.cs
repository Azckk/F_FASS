using Common.Service.Models;

namespace FASS.Service.Models.FlowExtend
{
    public class TaskTemplateMdcs : AuditModel
    {
        public required string CarTypeId { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double Priority { get; set; }

        public virtual List<TaskTemplateRule> TaskTemplateRules { get; set; } = [];

    }
}
