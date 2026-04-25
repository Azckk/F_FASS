using Common.Service.Models;

namespace FASS.Service.Models.RecordExtend
{
    public class AlarmMdcs : AuditModel
    {
        public required string CarCode { get; set; }
        public string? CarName { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsFinish { get; set; } = false;
    }
}
