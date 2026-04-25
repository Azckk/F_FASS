using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class MaterialStorageEntityType : AuditEntityType<MaterialStorageEntity>
    {
        public override void Configure(EntityTypeBuilder<MaterialStorageEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_material_storage");

            builder.Property(e => e.MaterialId).HasColumnName("material_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.StorageId).HasColumnName("storage_id").HasColumnType("varchar(50)").IsRequired();

            builder.HasOne(e => e.Material).WithMany(e => e.MaterialStorages).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(e => e.Storage).WithMany(e => e.MaterialStorages).HasForeignKey(e => e.StorageId).OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(e => e.MaterialId);
            builder.HasIndex(e => e.StorageId);
        }
    }
}
