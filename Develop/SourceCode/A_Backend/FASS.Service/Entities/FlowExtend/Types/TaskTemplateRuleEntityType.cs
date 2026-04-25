using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.FlowExtend.Types
{
    public class TaskTemplateRuleEntityType : AuditEntityType<TaskTemplateRuleEntity>
    {
        public override void Configure(EntityTypeBuilder<TaskTemplateRuleEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("flow_mdcs_task_template_rule");

            builder.Property(e => e.TaskTemplateId).HasColumnName("task_template_id").HasColumnType("varchar(50)").IsRequired();

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            builder.Property(e => e.Value).HasColumnName("value").HasColumnType("text").IsRequired();

            builder.HasOne(e => e.TaskTemplateMdcs).WithMany(e => e.TaskTemplateRules).HasForeignKey(e => e.TaskTemplateId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.TaskTemplateId);
        }
    }
}
