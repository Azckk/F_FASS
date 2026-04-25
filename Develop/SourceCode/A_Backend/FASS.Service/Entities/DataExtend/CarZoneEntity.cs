using Common.Service.Entities;

namespace FASS.Service.Entities.DataExtend
{
    public class CarZoneEntity : AuditEntity
    {
        public required string CarId { get; set; }
        public required string ZoneId { get; set; }

        //public virtual CarEntity Car { get; set; }
        //public virtual ZoneEntity Zone { get; set; }
    }
}
