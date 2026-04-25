using Common.Service.Models;

namespace FASS.Service.Models.DataExtend
{
    public class CarZone : AuditModel
    {
        public required string CarId { get; set; }
        public required string ZoneId { get; set; }

        //public virtual Car Car { get; set; }
        //public virtual Zone Zone { get; set; }
    }
}
