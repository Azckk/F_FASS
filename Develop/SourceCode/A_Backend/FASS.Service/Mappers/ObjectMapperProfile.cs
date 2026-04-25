using AutoMapper;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Dtos.Object;
using FASS.Service.Entities.Object;
using FASS.Service.Models.Object;

namespace FASS.Service.Mappers
{
    public class ObjectMapperProfile : Profile
    {
        public ObjectMapperProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<ThirdpartySystemEntity, ThirdpartySystemDto>().ReverseMap();

            CreateMap<AutoDoorEntity, AutoDoorDto>().ReverseMap();


            CreateMap<SafetyLightGridEntity, SafetyLightGridDto>().ReverseMap();
            CreateMap<SafetyLightGridsItemEntity, SafetyLightGridsItemDto>().ReverseMap();

            CreateMap<SafetyLightGridsItemEntity, SafetyLightGridsItem>();
            CreateMap<SafetyLightGridsItem, SafetyLightGridsItemEntity>()
                .ForMember(dest => dest.safetyLightGrid, opt => opt.Ignore());



            CreateMap<TrafficLightEntity, TrafficLightDto>().ReverseMap();
            CreateMap<TrafficLightItemEntity, TrafficLightItemDto>().ReverseMap();

            CreateMap<TrafficLightItemEntity, TrafficLightItem>();
            CreateMap<TrafficLightItem, TrafficLightItemEntity>()
                .ForMember(dest => dest.trafficLightEntity, opt => opt.Ignore());


            CreateMap<ButtonBoxEntity, ButtonBoxDto>().ReverseMap();
            CreateMap<ButtonBoxItemEntity, ButtonBoxItemDto>().ReverseMap();

            CreateMap<ButtonBoxItemEntity, ButtonBoxItem>();
            CreateMap<ButtonBoxItem, ButtonBoxItemEntity>()
                .ForMember(dest => dest.buttonBoxEntity, opt => opt.Ignore());

            CreateMap<ElevatorEntity, ElevatorDto>().ReverseMap();
            CreateMap<ElevatorItemEntity, ElevatorItemDto>().ReverseMap();

            CreateMap<ElevatorItemEntity, ElevatorItem>();
            CreateMap<ElevatorItem, ElevatorItemEntity>()
                .ForMember(dest => dest.elevatorEntity, opt => opt.Ignore());
        }
    }
}
