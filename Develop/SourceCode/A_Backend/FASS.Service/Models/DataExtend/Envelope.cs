using Common.Service.Models;

namespace FASS.Service.Models.DataExtend
{
    public class Envelope : AuditModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Value { get; set; }
    }
}
