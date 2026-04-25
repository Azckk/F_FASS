using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace FASS.Service.Entities.Object.Types
{
    public class ElevatorItemEntityType : AuditEntityType<ElevatorItemEntity>
    {

        public override void Configure(EntityTypeBuilder<ElevatorItemEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_elevator_item");
            builder.Property(e => e.ElevatorId)
                   .HasColumnName("elevator_id")
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

            builder.HasOne(e => e.elevatorEntity).WithMany(e=>e.ElevatorEntityItems).HasForeignKey(e => e.ElevatorId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(e => e.ElevatorId);
        }
    }
}
