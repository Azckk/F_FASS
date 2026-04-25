using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.RecordExtend.Types
{
    public class AlarmMdcsEntityType : AuditEntityType<AlarmMdcsEntity>
    {
        public override void Configure(EntityTypeBuilder<AlarmMdcsEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("record_mdcs_alarm");

            builder.Property(e => e.CarCode).HasColumnName("car_code").HasColumnType("varchar(50)").HasComment("车辆编号").IsRequired();
            builder.Property(e => e.CarName).HasColumnName("car_name").HasColumnType("varchar(50)").HasComment("车辆名称");
            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").HasComment("报警代码").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(100)").HasComment("报警名称");
            builder.Property(e => e.StartTime).HasColumnName("start_time").HasColumnType("timestamp").HasComment("开始时间").IsRequired();
            builder.Property(e => e.EndTime).HasColumnName("end_time").HasColumnType("timestamp").HasComment("结束时间");
            builder.Property(e => e.IsFinish).HasColumnName("is_finish").HasColumnType("bool").HasComment("结束标记");
            builder.HasIndex(e => e.CarCode);
            builder.HasIndex(e => e.StartTime);
            builder.HasIndex(e => e.IsFinish);
            //builder.HasIndex(e => new { e.StartTime, e.IsFinish });
        }
    }
}
