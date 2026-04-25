using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace FASS.Service.Entities.Object.Types
{
    public class TrafficLightItemEntityType : AuditEntityType<TrafficLightItemEntity>
    {

        public override void Configure(EntityTypeBuilder<TrafficLightItemEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_traffic_light_item");
            builder.Property(e => e.TrafficLightId)
                   .HasColumnName("traffic_light_id")
                   .HasColumnType("varchar(50)")
                   .IsRequired();

            builder.Property(e => e.OpenCloseSignal)
                   .HasColumnName("open_close_signal")
                   .HasColumnType("varchar(50)")
                   .IsRequired();

            builder.Property(e => e.Value)
                   .HasColumnName("value")
                   .HasColumnType("varchar(50)");

            builder.Property(e => e.State)
                   .HasColumnName("state")
                   .HasColumnType("varchar(50)");


            builder.Property(e => e.Station)
                   .HasColumnName("station")
                   .HasColumnType("varchar(50)")
                   .IsRequired();

            builder.HasOne(e => e.trafficLightEntity).WithMany(e=>e.TrafficLightItems).HasForeignKey(e => e.TrafficLightId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(e => e.TrafficLightId);
        }
    }
}
