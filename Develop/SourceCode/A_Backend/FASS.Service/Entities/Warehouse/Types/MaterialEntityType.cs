using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class MaterialEntityType : AuditEntityType<MaterialEntity>
    {
        public override void Configure(EntityTypeBuilder<MaterialEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_material");

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");

            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.IsLock).HasColumnName("is_lock").HasColumnType("bool").IsRequired();

            builder.Property(e => e.Barcode).HasColumnName("barcode").HasColumnType("text");

            builder.Property(e => e.Batch).HasColumnName("batch").HasColumnType("varchar(50)");
            builder.Property(e => e.Spec).HasColumnName("spec").HasColumnType("varchar(50)");
            builder.Property(e => e.Unit).HasColumnName("unit").HasColumnType("varchar(50)");

            builder.Property(e => e.Quantity).HasColumnName("quantity").HasColumnType("int4");

            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
