using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class ContainerEntityType : AuditEntityType<ContainerEntity>
    {
        public override void Configure(EntityTypeBuilder<ContainerEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_container");

            builder.Property(e => e.AreaId).HasColumnName("area_id").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.IsLock).HasColumnName("is_lock").HasColumnType("bool").IsRequired();

            builder.Property(e => e.Barcode).HasColumnName("barcode").HasColumnType("text");

            builder.Property(e => e.Length).HasColumnName("length").HasColumnType("float8");
            builder.Property(e => e.Width).HasColumnName("width").HasColumnType("float8");
            builder.Property(e => e.Height).HasColumnName("height").HasColumnType("float8");

            builder.HasOne(e => e.Area).WithMany(e => e.Containers).HasForeignKey(e => e.AreaId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.AreaId);
            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
