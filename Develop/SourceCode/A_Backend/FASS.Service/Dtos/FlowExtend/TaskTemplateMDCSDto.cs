using Common.Service.Dtos;

namespace FASS.Service.Dtos.FlowExtend
{
    public class TaskTemplateMdcsDto : AuditDto
    {
        public required string CarTypeId { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double Priority { get; set; }
        public string? CarTypeCode { get; set; }
        public string? CarTypeName { get; set; }
    }
}
