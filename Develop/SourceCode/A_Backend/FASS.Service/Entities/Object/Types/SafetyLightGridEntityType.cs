using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Service.Entities.Types;
using FASS.Data.Entities.Warehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.Object.Types
{
    public class SafetyLightGridEntityType : AuditEntityType<SafetyLightGridEntity>
    {
        public override void Configure(EntityTypeBuilder<SafetyLightGridEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_safety_light_grids");

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
