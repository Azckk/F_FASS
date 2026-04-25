using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace FASS.Service.Entities.Setting.Types
{
    public class ConfigTrafficControlEntityType : AuditEntityType<ConfigTrafficControlEntity>
    {
        public override void Configure(EntityTypeBuilder<ConfigTrafficControlEntity> builder)
        {
            base.Configure(builder);
            // 设置表名
            builder.ToTable("set_config_taffic");
            // 配置列属性
            builder
                .Property(e => e.Key)
                .HasColumnName("key")
                .HasColumnType("varchar(50)")
                .HasComment("配置参数名")
                .IsRequired();

            builder
                .Property(e => e.Value)
                .HasColumnName("value")
                .HasColumnType("varchar(50)")
                .HasComment("参数值")
                .IsRequired();
        }
    }
}
