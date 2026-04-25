using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FASS.Service.Entities.Warehouse.Types
{
    public class PreWorkEntityType : AuditEntityType<PreWorkEntity>
    {
        public override void Configure(EntityTypeBuilder<PreWorkEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_pre_work");

            builder.Property(e => e.ContainerId).HasColumnName("container_id").HasColumnType("varchar(50)");
            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("text").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("text");
            builder.Property(e => e.SrcStorageId).HasColumnName("src_storage_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.DestStorageId).HasColumnName("dest_storage_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.MaterialCode).HasColumnName("material_code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.HasIndex(e => e.ContainerId);
            builder.HasIndex(e => e.MaterialCode);
            builder.HasIndex(e => e.Code);
        }
    }
}
