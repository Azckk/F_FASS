using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.Warehouse.Types
{
    public class TagEntityType : AuditEntityType<TagEntity>
    {
        public override void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("warehouse_tag");

            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)");
            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Value).HasColumnName("value").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Colour).HasColumnName("colour").HasColumnType("varchar(50)").IsRequired();

            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasIndex(e => e.Name).IsUnique();
        }
    }


}
