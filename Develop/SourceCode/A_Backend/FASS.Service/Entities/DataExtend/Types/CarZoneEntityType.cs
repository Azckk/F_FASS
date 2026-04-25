using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.DataExtend.Types
{
    public class CarZoneEntityType : AuditEntityType<CarZoneEntity>
    {
        public override void Configure(EntityTypeBuilder<CarZoneEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_car_zone");

            builder.Property(e => e.CarId).HasColumnName("car_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.ZoneId).HasColumnName("zone_id").HasColumnType("varchar(50)").IsRequired();

            builder.HasIndex(e => e.CarId);
            builder.HasIndex(e => e.ZoneId);
        }
    }
}
