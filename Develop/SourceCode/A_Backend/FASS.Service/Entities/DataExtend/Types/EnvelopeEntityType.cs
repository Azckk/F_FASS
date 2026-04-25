using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.DataExtend.Types
{
    public class EnvelopeEntityType : AuditEntityType<EnvelopeEntity>
    {
        public override void Configure(EntityTypeBuilder<EnvelopeEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_envelope");

            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)").HasComment("属性名称").IsRequired();
            builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text").HasComment("属性描述").IsRequired();
            builder.Property(e => e.Value).HasColumnName("value").HasColumnType("varchar(100)").HasComment("参数值").IsRequired();

            builder.HasIndex(e => e.Name).IsUnique();
        }
    }
}
