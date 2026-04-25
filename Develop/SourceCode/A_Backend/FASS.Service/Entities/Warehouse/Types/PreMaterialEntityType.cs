using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.Warehouse.Types
{
    public class PreMaterialEntityType : AuditEntityType<PreMaterialEntity>
    {
        public override void Configure(EntityTypeBuilder<PreMaterialEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_pre_material");
            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");
            builder.Property(e => e.StorageId).HasColumnName("storage_id").HasColumnType("varchar(50)");
            builder.Property(e => e.StorageName).HasColumnName("storage_name").HasColumnType("varchar(50)");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.IsLock).HasColumnName("is_lock").HasColumnType("bool").IsRequired();
            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
