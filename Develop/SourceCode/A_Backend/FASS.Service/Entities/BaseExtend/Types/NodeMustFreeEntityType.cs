using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.BaseExtend.Types
{
    public class NodeMustFreeEntityType : AuditEntityType<NodeMustFreeEntity>
    {
        public override void Configure(EntityTypeBuilder<NodeMustFreeEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("base_node_must_free");
            builder.Property(e => e.NodeCode).HasColumnName("node_code").HasColumnType("varchar(50)").HasComment("站点编码").IsRequired();
            builder.Property(e => e.MustFreeNodeCode).HasColumnName("must_free_node_code").HasColumnType("varchar(50)").HasComment("关联必空站点编码").IsRequired();
        }
    }
}
