using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.FlowExtend.Types
{
    public class TaskTemplateMdcsEntityType : AuditEntityType<TaskTemplateMdcsEntity>
    {
        public override void Configure(EntityTypeBuilder<TaskTemplateMdcsEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("flow_mdcs_task_template");

            builder.Property(e => e.CarTypeId).HasColumnName("car_type_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)");
            builder.Property(e => e.Priority).HasColumnName("priority").HasColumnType("float8");

            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
