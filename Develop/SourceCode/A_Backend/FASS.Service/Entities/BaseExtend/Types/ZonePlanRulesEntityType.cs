using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.BaseExtend.Types
{
    public class ZonePlanRulesEntityType : AuditEntityType<ZonePlanRulesEntity>
    {
        public override void Configure(EntityTypeBuilder<ZonePlanRulesEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("base_zone_plan_rules");
            builder.Property(e => e.ZoneId).HasColumnName("zone_id").HasColumnType("varchar(50)").HasComment("区域id").IsRequired();
            builder.Property(e => e.RuleName).HasColumnName("rule_name").HasColumnType("varchar(50)").HasComment("规则名称").IsRequired();
        }
    }
}
