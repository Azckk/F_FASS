using Common.Service.Dtos;

namespace FASS.Service.Dtos.RecordExtend
{
    public class AlarmMdcsDto : AuditDto
    {
        public required string CarCode { get; set; }
        public string? CarName { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsFinish { get; set; }
        public string? Continue { get; set; }//持续时间
    }
}
