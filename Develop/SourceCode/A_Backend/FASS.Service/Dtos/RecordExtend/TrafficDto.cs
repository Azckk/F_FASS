using Common.Service.Dtos;

namespace FASS.Service.Dtos.RecordExtend
{
    public class TrafficDto : AuditDto
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
        public string? Continue { get; set; }//持续时间

    }
}
