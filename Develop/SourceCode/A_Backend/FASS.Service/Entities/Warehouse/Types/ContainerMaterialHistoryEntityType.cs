using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class ContainerMaterialHistoryEntityType : AuditEntityType<ContainerMaterialHistoryEntity>
    {
        public override void Configure(EntityTypeBuilder<ContainerMaterialHistoryEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_container_material_history");

            builder.Property(e => e.ContainerId).HasColumnName("container_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.MaterialId).HasColumnName("material_id").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.HasOne(e => e.Container).WithMany(e => e.ContainerMaterialHistorys).HasForeignKey(e => e.ContainerId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Material).WithMany(e => e.ContainerMaterialHistorys).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.ContainerId);
            builder.HasIndex(e => e.MaterialId);
        }
    }
}
