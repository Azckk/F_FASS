using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.BaseExtend.Types
{
    public class NodeAttributeEntityType : AuditEntityType<NodeAttributeEntity>
    {
        public override void Configure(EntityTypeBuilder<NodeAttributeEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("base_node_attribute");
            builder.Property(e => e.NodeId).HasColumnName("node_id").HasColumnType("varchar(50)").HasComment("站点id").IsRequired();
            builder.Property(e => e.AttributeType).HasColumnName("attribute_type").HasColumnType("varchar(50)").HasComment("属性类型").IsRequired();
            builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text").HasComment("属性描述");
            builder.Property(e => e.Value).HasColumnName("value").HasColumnType("text").HasComment("属性值");
        }
    }
}
