using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.Object.Types
{
    public class AutoDoorEntityType : AuditEntityType<AutoDoorEntity>
    {
        public override void Configure(EntityTypeBuilder<AutoDoorEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_door");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("varchar(50)");
            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)");
            builder.Property(e => e.PrevNodeId).HasColumnName("prev_node_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.NextNodeId).HasColumnName("next_node_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.IsAdvance).HasColumnName("is_advance").HasColumnType("bool");
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").IsRequired();

            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
