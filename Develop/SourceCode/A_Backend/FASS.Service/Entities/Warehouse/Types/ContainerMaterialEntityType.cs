using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class ContainerMaterialEntityType : AuditEntityType<ContainerMaterialEntity>
    {
        public override void Configure(EntityTypeBuilder<ContainerMaterialEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_container_material");

            builder.Property(e => e.ContainerId).HasColumnName("container_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.MaterialId).HasColumnName("material_id").HasColumnType("varchar(50)").IsRequired();

            builder.HasOne(e => e.Container).WithMany(e => e.ContainerMaterials).HasForeignKey(e => e.ContainerId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(e => e.Material).WithMany(e => e.ContainerMaterials).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(e => e.ContainerId);
            builder.HasIndex(e => e.MaterialId);
        }
    }
}
