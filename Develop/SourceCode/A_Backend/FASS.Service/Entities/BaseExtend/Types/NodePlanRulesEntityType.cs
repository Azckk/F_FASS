using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.BaseExtend.Types
{
    public class NodePlanRulesEntityType : AuditEntityType<NodePlanRulesEntity>
    {
        public override void Configure(EntityTypeBuilder<NodePlanRulesEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("base_node_plan_rules");
            builder.Property(e => e.NodeId).HasColumnName("node_id").HasColumnType("varchar(50)").HasComment("站点id").IsRequired();
            builder.Property(e => e.RuleName).HasColumnName("rule_name").HasColumnType("varchar(50)").HasComment("规则名称").IsRequired();
        }
    }
}
