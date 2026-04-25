using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.RecordExtend.Types
{
    public class DisChargeConsumeEntityType : AuditEntityType<DisChargeConsumeEntity>
    {
        public override void Configure(EntityTypeBuilder<DisChargeConsumeEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("report_discharge_consume");
            builder.Property(x => x.CarCode).HasColumnName("car_code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.LastDN).HasColumnName("last_dn").HasColumnType("float").IsRequired();
            builder.Property(e => e.CurrentDN).HasColumnName("current_dn").HasColumnType("float").IsRequired();
            builder.Property(e => e.ConsumeDN).HasColumnName("consume_dn").HasColumnType("float").IsRequired();
        }

    }
}
