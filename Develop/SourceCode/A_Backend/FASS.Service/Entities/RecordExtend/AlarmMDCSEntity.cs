using Common.Service.Entities;

namespace FASS.Service.Entities.RecordExtend
{
    public class AlarmMdcsEntity : AuditEntity
    {
        public required string CarCode { get; set; }
        public string? CarName { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsFinish { get; set; }
    }
}
