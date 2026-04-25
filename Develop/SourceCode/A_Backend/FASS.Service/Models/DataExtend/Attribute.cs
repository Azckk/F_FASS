using Common.Service.Models;

namespace FASS.Service.Models.DataExtend
{
    public class Attribute : AuditModel
    {
        public string? Kind { get; set; }
        public string? Type { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
    }
}
