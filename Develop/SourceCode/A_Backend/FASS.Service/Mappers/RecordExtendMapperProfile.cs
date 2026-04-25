using AutoMapper;
using FASS.Service.Dtos.RecordExtend;
using FASS.Service.Entities.RecordExtend;
using FASS.Service.Models.RecordExtend;

namespace FASS.Service.Mappers
{
    public class RecordExtendMapperProfile : Profile
    {
        public RecordExtendMapperProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<TrafficEntity, TrafficDto>().ReverseMap();
            CreateMap<AlarmMdcsEntity, AlarmMdcsDto>().ReverseMap();

            CreateMap<TrafficEntity, Traffic>().ReverseMap();
            CreateMap<AlarmMdcsEntity, AlarmMdcs>().ReverseMap();

            CreateMap<ChargeConsumeEntity, ChargeConsumeDto>().ReverseMap();
            CreateMap<ChargeConsumeEntity, ChargeConsume>().ReverseMap();

            CreateMap<DisChargeConsumeEntity, DisChargeConsumeDto>().ReverseMap();
            CreateMap<DisChargeConsumeEntity, DisChargeConsume>().ReverseMap();
        }
    }
}
