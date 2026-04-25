using AutoMapper;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Entities.DataExtend;
using FASS.Service.Models.DataExtend;
using Attribute = FASS.Service.Models.DataExtend.Attribute;

namespace FASS.Service.Mappers
{
    public class DataExtendMapperProfile : Profile
    {
        public DataExtendMapperProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<EnvelopeEntity, EnvelopeDto>().ReverseMap();
            CreateMap<ChargingStationEntity, ChargingStationDto>().ReverseMap();
            CreateMap<TrafficRulesEntity, TrafficRulesDto>().ReverseMap();
            CreateMap<PlanRulesEntity, PlanRulesDto>().ReverseMap();
            CreateMap<AttributeEntity, AttributeDto>().ReverseMap();
            CreateMap<CarZoneEntity, CarZoneDto>().ReverseMap();

            CreateMap<EnvelopeEntity, Envelope>().ReverseMap();
            CreateMap<ChargingStationEntity, ChargingStation>().ReverseMap();
            CreateMap<TrafficRulesEntity, TrafficRules>().ReverseMap();
            CreateMap<PlanRulesEntity, PlanRules>().ReverseMap();
            CreateMap<AttributeEntity, Attribute>().ReverseMap();
            CreateMap<CarZoneEntity, CarZone>().ReverseMap();
        }
    }


}
