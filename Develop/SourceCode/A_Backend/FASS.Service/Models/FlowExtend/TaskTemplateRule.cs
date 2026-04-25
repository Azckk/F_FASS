using Common.Service.Models;

namespace FASS.Service.Models.FlowExtend
{
    public class TaskTemplateRule : AuditModel
    {
        public required string TaskTemplateId { get; set; }

        public required string Code { get; set; }
        public string? Description { get; set; }
        public required string Value { get; set; }
    }
}
