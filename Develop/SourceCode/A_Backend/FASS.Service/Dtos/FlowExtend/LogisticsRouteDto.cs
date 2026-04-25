using Common.Service.Dtos;

namespace FASS.Service.Dtos.FlowExtend
{
    public class LogisticsRouteDto : AuditDto
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string SrcAreaId { get; set; }
        public required string DestAreaId { get; set; }

        public string? SrcAreaCode { get; set; }
        public string? DestAreaCode { get; set; }
    }

}
