using Common.Service.Entities.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FASS.Service.Entities.DataExtend.Types
{
    public class ChargingStationEntityType : AuditEntityType<ChargingStationEntity>
    {
        public override void Configure(EntityTypeBuilder<ChargingStationEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("data_charging_station");

            builder.Property(e => e.ChargeId).HasColumnName("charge_id").HasColumnType("varchar(50)").HasComment("充电点id").IsRequired();
            builder.Property(e => e.ChargeCode).HasColumnName("charge_code").HasColumnType("varchar(50)").HasComment("充电点编码").IsRequired();
            builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(50)").HasComment("编码").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)").HasComment("名称");
            builder.Property(e => e.Ip).HasColumnName("ip").HasColumnType("varchar(50)").HasComment("IP地址").IsRequired();
            builder.Property(e => e.Port).HasColumnName("port").HasColumnType("int4").HasComment("端口").IsRequired();
            builder.Property(e => e.Protocol).HasColumnName("protocol").HasColumnType("varchar(50)").HasComment("通讯协议").IsRequired();
            builder.Property(e => e.Mode).HasColumnName("mode").HasColumnType("varchar(50)").HasComment("充电模式").IsRequired();
            builder.Property(e => e.Voltage).HasColumnName("voltage").HasColumnType("float8").HasComment("额定电压").IsRequired();
            builder.Property(e => e.Current).HasColumnName("current").HasColumnType("float8").HasComment("额定电流").IsRequired();
            builder.Property(e => e.IsOccupied).HasColumnName("is_occupied").HasColumnType("bool").HasComment("是否占用");
            builder.Property(e => e.OccupiedCarId).HasColumnName("occupied_car_id").HasColumnType("varchar(50)").HasComment("占用车辆id");
            builder.Property(e => e.State).HasColumnName("state").HasColumnType("varchar(50)").HasComment("状态");

            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasIndex(e => e.ChargeId).IsUnique();
        }
    }

}
