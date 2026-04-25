using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class StorageContainerEntityType : AuditEntityType<StorageContainerEntity>
    {
        public override void Configure(EntityTypeBuilder<StorageContainerEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_storage_container");

            builder.Property(e => e.StorageId).HasColumnName("storage_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.ContainerId).HasColumnName("container_id").HasColumnType("varchar(50)").IsRequired();

            builder.HasOne(e => e.Storage).WithMany(e => e.StorageContainers).HasForeignKey(e => e.StorageId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(e => e.Container).WithMany(e => e.StorageContainers).HasForeignKey(e => e.ContainerId).OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(e => e.StorageId);
            builder.HasIndex(e => e.ContainerId);
        }
    }
}
