using Common.Service.Models;

namespace FASS.Service.Models.BaseExtend
{
    public class NodeMustFree : AuditModel
    {
        public required string NodeCode { get; set; }
        public required string MustFreeNodeCode { get; set; }

    }
}
