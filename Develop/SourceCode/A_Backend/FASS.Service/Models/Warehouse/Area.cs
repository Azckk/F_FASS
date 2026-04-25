using Common.Service.Models;

namespace FASS.Data.Models.Warehouse
{
    public class Area : AuditModel
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

        public virtual List<Storage> Storages { get; set; } = [];

        public virtual List<Container> Containers { get; set; } = [];
    }
}
