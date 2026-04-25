using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.Object.Types
{
    public class ThirdpartySystemEntityType : AuditEntityType<ThirdpartySystemEntity>
    {
        public override void Configure(EntityTypeBuilder<ThirdpartySystemEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_interface");
            //builder.Property(x => x.Id).HasMaxLength(50).IsRequired();

            builder
                .Property(x => x.SystemName)
                .HasColumnName("system_name").
                HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(50)").IsRequired();

            builder
                .Property(x => x.CommunicatioMode)
                .HasColumnName("communicatio_mode")
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(x => x.Url).HasColumnName("url").HasColumnType("varchar(200)").IsRequired();

            builder.Property(x => x.Method).HasColumnName("method").HasColumnType("varchar(50)").IsRequired();

            builder.Property(x => x.Headers).HasColumnName("headers").HasColumnType("text"); // text类型的列通常可以存储非常大的数据

            builder
                .Property(x => x.QueryParams)
                .HasColumnName("query_params")
                .HasColumnType("text"); // text类型的列通常可以存储非常大的数据

            builder
                .Property(x => x.ResponseParams)
                .HasColumnName("response_params")
                .HasColumnType("text"); // text类型的列通常可以存储非常大的数据

            builder.Property(x => x.State).HasColumnName("state").HasColumnType("text");

            builder.Property(x => x.IsRetransmit).HasColumnName("is_retransmit");

            //builder.Property(x => x.SortNumber).HasColumnType("float8"); // 对于非标准的数据库类型，需要指定列类型
        }
    }
}
