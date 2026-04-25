using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class StorageEntityType : AuditEntityType<StorageEntity>
    {
        public override void Configure(EntityTypeBuilder<StorageEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_storage");

            builder.Property(e => e.AreaId).HasColumnName("area_id").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.NodeId).HasColumnName("node_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.NodeCode).HasColumnName("node_code").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.IsLock).HasColumnName("is_lock").HasColumnType("bool").IsRequired();

            builder.Property(e => e.Barcode).HasColumnName("barcode").HasColumnType("text");

            builder.HasOne(e => e.Area).WithMany(e => e.Storages).HasForeignKey(e => e.AreaId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.AreaId);
            builder.HasIndex(e => e.NodeId);
            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
