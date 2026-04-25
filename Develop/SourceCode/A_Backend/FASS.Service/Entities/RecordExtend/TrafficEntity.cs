using Common.Service.Entities;

namespace FASS.Service.Entities.RecordExtend
{
    public class TrafficEntity : AuditEntity
    {
        public required string FromCarCode { get; set; }
        public required string ToCarCode { get; set; }
        public string? FromCarName { get; set; }
        public string? ToCarName { get; set; }
        public required string LockedNodes { get; set; }
        public string? State { get; set; }
        public string? Info { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsFinish { get; set; }
    }
}
