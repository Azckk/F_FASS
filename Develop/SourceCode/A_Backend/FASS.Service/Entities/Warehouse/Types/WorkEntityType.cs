using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Data.Entities.Warehouse.Types
{
    public class WorkEntityType : AuditEntityType<WorkEntity>
    {
        public override void Configure(EntityTypeBuilder<WorkEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_work");

            builder.Property(e => e.ContainerId).HasColumnName("container_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.AreaId).HasColumnName("area_id").HasColumnType("varchar(50)");

            builder.Property(e => e.TaskId).HasColumnName("task_id").HasColumnType("varchar(50)");
            builder.Property(e => e.TaskCode).HasColumnName("task_code").HasColumnType("varchar(50)");

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("text").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("text");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.HasOne(e => e.Container).WithMany(e => e.Works).HasForeignKey(e => e.ContainerId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.ContainerId);
            builder.HasIndex(e => e.TaskId);
            builder.HasIndex(e => e.Code);
        }
    }
}
