using Common.Service.Models;

namespace FASS.Service.Models.Warehouse
{
    public class Tag : AuditModel
    {
        public string? Type { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
        public required string Colour { get; set; }

    }
}
