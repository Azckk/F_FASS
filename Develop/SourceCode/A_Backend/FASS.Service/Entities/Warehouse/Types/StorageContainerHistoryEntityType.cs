using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class StorageContainerHistoryEntityType : AuditEntityType<StorageContainerHistoryEntity>
    {
        public override void Configure(EntityTypeBuilder<StorageContainerHistoryEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_storage_container_history");

            builder.Property(e => e.StorageId).HasColumnName("storage_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.ContainerId).HasColumnName("container_id").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.HasOne(e => e.Storage).WithMany(e => e.StorageContainerHistorys).HasForeignKey(e => e.StorageId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Container).WithMany(e => e.StorageContainerHistorys).HasForeignKey(e => e.ContainerId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.StorageId);
            builder.HasIndex(e => e.ContainerId);
        }
    }
}
