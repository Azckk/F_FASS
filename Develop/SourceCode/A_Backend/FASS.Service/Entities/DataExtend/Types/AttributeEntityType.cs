using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.DataExtend.Types
{
    public class AttributeEntityType : AuditEntityType<AttributeEntity>
    {
        public override void Configure(EntityTypeBuilder<AttributeEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_attribute");
            builder.Property(e => e.Kind).HasColumnName("kind").HasColumnType("varchar(50)").HasComment("元素种类");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)").HasComment("类型");
            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").HasComment("编码").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)").HasComment("名称");

            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
