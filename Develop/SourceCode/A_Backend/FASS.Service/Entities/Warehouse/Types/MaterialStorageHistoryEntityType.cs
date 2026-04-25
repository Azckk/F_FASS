using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class MaterialStorageHistoryEntityType : AuditEntityType<MaterialStorageHistoryEntity>
    {
        public override void Configure(EntityTypeBuilder<MaterialStorageHistoryEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_material_storage_history");

            builder.Property(e => e.MaterialId).HasColumnName("material_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.StorageId).HasColumnName("storage_id").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.HasOne(e => e.Material).WithMany(e => e.MaterialStorageHistorys).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Storage).WithMany(e => e.MaterialStorageHistorys).HasForeignKey(e => e.StorageId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.MaterialId);
            builder.HasIndex(e => e.StorageId);
        }
    }
}
