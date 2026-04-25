using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace FASS.Service.Entities.Object.Types
{
    public class ButtonBoxItemEntityType : AuditEntityType<ButtonBoxItemEntity>
    {

        public override void Configure(EntityTypeBuilder<ButtonBoxItemEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_button_box_item");
            builder.Property(e => e.ButtonBoxId)
                   .HasColumnName("button_box_id")
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

            builder.HasOne(e => e.buttonBoxEntity).WithMany(e=>e.buttonBoxItems).HasForeignKey(e => e.ButtonBoxId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(e => e.ButtonBoxId);
        }
    }
}
