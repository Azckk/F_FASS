using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.FlowExtend.Types
{
    public class LogisticsRouteEntityType : AuditEntityType<LogisticsRouteEntity>
    {
        public override void Configure(EntityTypeBuilder<LogisticsRouteEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("flow_logistics_route");

            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");

            builder.Property(e => e.SrcAreaId).HasColumnName("src_area_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.DestAreaId).HasColumnName("dest_area_id").HasColumnType("varchar(50)").IsRequired();

            builder.HasIndex(e => e.Code).IsUnique();
        }
    }

}
