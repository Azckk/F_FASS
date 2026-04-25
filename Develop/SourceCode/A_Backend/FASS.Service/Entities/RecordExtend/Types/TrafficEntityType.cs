using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.RecordExtend.Types
{
    public class TrafficEntityType : AuditEntityType<TrafficEntity>
    {
        public override void Configure(EntityTypeBuilder<TrafficEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("record_car_traffic");

            builder.Property(e => e.FromCarCode).HasColumnName("from_car_code").HasColumnType("varchar(50)").HasComment("管制车辆编号").IsRequired();
            builder.Property(e => e.ToCarCode).HasColumnName("to_car_code").HasColumnType("varchar(50)").HasComment("被管制车辆编号").IsRequired();
            builder.Property(e => e.FromCarName).HasColumnName("from_car_name").HasColumnType("varchar(50)").HasComment("管制车辆名称");
            builder.Property(e => e.ToCarName).HasColumnName("to_car_name").HasColumnType("varchar(50)").HasComment("被管制车辆名称");
            builder.Property(e => e.LockedNodes).HasColumnName("locked_nodes").HasColumnType("varchar(100)").HasComment("被锁站点").IsRequired();
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("text").HasComment("交管状态");
            builder.Property(e => e.Info).HasColumnName("info").HasColumnType("text").HasComment("阻塞详情");
            builder.Property(e => e.StartTime).HasColumnName("start_time").HasColumnType("timestamp").HasComment("开始时间").IsRequired();
            builder.Property(e => e.EndTime).HasColumnName("end_time").HasColumnType("timestamp").HasComment("结束时间");
            builder.Property(e => e.IsFinish).HasColumnName("is_finish").HasColumnType("bool").HasComment("是否结束");
            builder.HasIndex(e => e.FromCarCode);
        }
    }
}
