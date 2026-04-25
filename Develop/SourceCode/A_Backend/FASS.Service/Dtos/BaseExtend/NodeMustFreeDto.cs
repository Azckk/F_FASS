using Common.Service.Dtos;

namespace FASS.Service.Dtos.BaseExtend
{
    public class NodeMustFreeDto : AuditDto
    {
        public required string NodeCode { get; set; }
        public required string MustFreeNodeCode { get; set; }

    }
}
