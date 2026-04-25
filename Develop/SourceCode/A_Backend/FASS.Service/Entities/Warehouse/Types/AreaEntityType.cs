using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class AreaEntityType : AuditEntityType<AreaEntity>
    {
        public override void Configure(EntityTypeBuilder<AreaEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_area");

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
