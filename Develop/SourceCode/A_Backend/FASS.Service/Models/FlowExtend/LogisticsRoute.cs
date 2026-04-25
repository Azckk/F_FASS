using Common.Service.Models;
using FASS.Data.Models.Warehouse;

namespace FASS.Service.Models.FlowExtend
{
    public class LogisticsRoute : AuditModel
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string SrcAreaId { get; set; }
        public required string DestAreaId { get; set; }
        public virtual Area SrcArea { get; set; } = null!;
        public virtual Area DestArea { get; set; } = null!;
    }
}
