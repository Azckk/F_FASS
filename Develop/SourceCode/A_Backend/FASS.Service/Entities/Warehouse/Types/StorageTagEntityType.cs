using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.Warehouse.Types
{
    public class StorageTagEntityType : AuditEntityType<StorageTagEntity>
    {
        public override void Configure(EntityTypeBuilder<StorageTagEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_storage_tag");

            builder.Property(e => e.StorageId).HasColumnName("storage_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.TagId).HasColumnName("tag_id").HasColumnType("varchar(50)").IsRequired();

            builder.HasOne(e => e.Storage).WithMany(e => e.StorageTags).HasForeignKey(e => e.StorageId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Tag).WithMany(e => e.StorageTags).HasForeignKey(e => e.TagId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.StorageId);
            builder.HasIndex(e => e.TagId);
        }
    }
}
