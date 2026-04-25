using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.DataExtend.Types
{
    public class PlanRulesEntityType : AuditEntityType<PlanRulesEntity>
    {
        public override void Configure(EntityTypeBuilder<PlanRulesEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_plan_rules");
            builder.Property(e => e.NodeId).HasColumnName("node_id").HasColumnType("varchar(50)").HasComment("站点id");
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)").HasComment("规则名称").IsRequired();
            builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text").HasComment("描述").IsRequired();
            builder.Property(e => e.Value).HasColumnName("value").HasColumnType("text").HasComment("配置参数值").IsRequired();

            builder.HasIndex(e => e.Name).IsUnique();
        }
    }
}
