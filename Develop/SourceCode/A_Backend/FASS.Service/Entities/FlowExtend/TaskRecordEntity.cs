using Common.Service.Entities;

namespace FASS.Service.Entities.FlowExtend
{
    public class TaskRecordEntity : AuditEntity
    {
        public required string TaskTemplateId { get; set; }
        public string? AppendId { get; set; }

        public string? CarId { get; set; }
        public string? CarTypeId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public required string SrcNodeId { get; set; }
        public required string DestNodeId { get; set; }
        public string? SrcNodeCode { get; set; }
        public string? DestNodeCode { get; set; }
        public string? SrcStorageId { get; set; }
        public string? DestStorageId { get; set; }
        public string? SrcAreaId { get; set; }
        public string? DestAreaId { get; set; }
        public int Priority { get; set; }
        public string? State { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? TaskCreateAt { get; set; } = DateTime.Now;
        public bool IsLoop { get; set; } = false;
        public string? Condition { get; set; }
        public string? CallMode { get; set; }
        public virtual TaskTemplateMdcsEntity TaskTemplateMdcs { get; set; } = null!;

    }
}
