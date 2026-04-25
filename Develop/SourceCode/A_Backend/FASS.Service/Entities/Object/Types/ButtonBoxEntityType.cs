using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace FASS.Service.Entities.Object.Types
{
    public class ButtonBoxEntityType : AuditEntityType<ButtonBoxEntity>
    {
        public override void Configure(EntityTypeBuilder<ButtonBoxEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_button_box");

            builder.Property(e => e.ProtocolType)
                   .HasColumnName("protocol_type")
                   .HasColumnType("varchar(50)")
                   .IsRequired();

            builder.Property(e => e.Ip)
                   .HasColumnName("ip")
                   .HasColumnType("varchar(50)")
                   .IsRequired();

            builder.Property(e => e.Port)
                   .HasColumnName("port")
                   .HasColumnType("varchar(50)")
                   .IsRequired();


            builder.Property(e => e.Name)
                   .HasColumnName("name")
                   .HasColumnType("varchar(50)")
                   .IsRequired();

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Brand).HasColumnName("brand").HasColumnType("varchar(50)");
            builder.HasIndex(e => e.Code).IsUnique();


        }
    }
}
