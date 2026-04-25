using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.Object.Types
{
    public class ElevatorEntityType : AuditEntityType<ElevatorEntity>
    {
        public override void Configure(EntityTypeBuilder<ElevatorEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_elevator");

            builder
                .Property(e => e.ProtocolType)
                .HasColumnName("protocol_type")
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(e => e.Ip)
                .HasColumnName("ip")
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(e => e.Port)
                .HasColumnName("port")
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(e => e.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(e => e.Code)
                .HasColumnName("code")
                .HasColumnType("varchar(50)")
                .IsRequired();
            builder.Property(e => e.Brand).HasColumnName("brand").HasColumnType("varchar(50)");
            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
