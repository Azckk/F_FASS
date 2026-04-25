using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FASS.Service.Entities.Object.Types
{
    public class SafetyLightGridsItemEntityType : AuditEntityType<SafetyLightGridsItemEntity>
    {

        public override void Configure(EntityTypeBuilder<SafetyLightGridsItemEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_safety_light_grids_item");
            builder.Property(e => e.SafetyLightGridsId)
                   .HasColumnName("safety_light_grids_id")
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

            builder.HasOne(e => e.safetyLightGrid).WithMany(e => e.SafetyLightGridsItems).HasForeignKey(e => e.SafetyLightGridsId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(e => e.SafetyLightGridsId);
        }
    }
}
