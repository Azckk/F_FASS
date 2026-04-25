using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.FlowExtend.Types
{
    public class TaskRecordEntityType : AuditEntityType<TaskRecordEntity>
    {
        public override void Configure(EntityTypeBuilder<TaskRecordEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("flow_mdcs_task");

            builder.Property(e => e.TaskTemplateId).HasColumnName("task_template_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.AppendId).HasColumnName("append_id").HasColumnType("varchar(50)");
            builder.Property(e => e.CarId).HasColumnName("car_id").HasColumnType("varchar(50)");
            builder.Property(e => e.CarTypeId).HasColumnName("car_type_id").HasColumnType("varchar(50)");

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)");
            builder.Property(e => e.Priority).HasColumnName("priority").HasColumnType("int4");
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)");
            builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text");

            builder.Property(e => e.SrcNodeId).HasColumnName("src_node_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.SrcNodeCode).HasColumnName("src_node_code").HasColumnType("varchar(50)");
            builder.Property(e => e.DestNodeId).HasColumnName("dest_node_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.DestNodeCode).HasColumnName("dest_node_code").HasColumnType("varchar(50)");
            builder.Property(e => e.SrcStorageId).HasColumnName("src_storage_id").HasColumnType("varchar(50)");
            builder.Property(e => e.DestStorageId).HasColumnName("dest_storage_id").HasColumnType("varchar(50)");
            builder.Property(e => e.SrcAreaId).HasColumnName("src_area_id").HasColumnType("varchar(50)");
            builder.Property(e => e.DestAreaId).HasColumnName("dest_area_id").HasColumnType("varchar(50)");

            builder.Property(e => e.StartTime).HasColumnName("start_time").HasColumnType("timestamp");
            builder.Property(e => e.EndTime).HasColumnName("end_time").HasColumnType("timestamp");
            builder.Property(e => e.TaskCreateAt).HasColumnName("task_create_at").HasColumnType("timestamp").IsRequired();
            builder.Property(e => e.IsLoop).HasColumnName("is_loop").HasColumnType("bool");
            builder.Property(e => e.Condition).HasColumnName("condition").HasColumnType("varchar(50)");
            builder.Property(e => e.CallMode).HasColumnName("call_mode").HasColumnType("varchar(50)");
            builder.HasOne(e => e.TaskTemplateMdcs).WithMany(e => e.TaskRecords).HasForeignKey(e => e.TaskTemplateId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}