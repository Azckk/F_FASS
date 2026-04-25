using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.Custom.Types
{
    public class DemoEntityType : AuditEntityType<DemoEntity>
    {
        public override void Configure(EntityTypeBuilder<DemoEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("custom_demo");

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.HasIndex(e => e.Code).IsUnique();

        }
    }
}
