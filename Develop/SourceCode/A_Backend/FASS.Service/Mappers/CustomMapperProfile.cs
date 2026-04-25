using AutoMapper;
using FASS.Service.Dtos.Custom;
using FASS.Service.Entities.Custom;
using FASS.Service.Models.Custom;

namespace FASS.Service.Mappers
{
    public class CustomMapperProfile : Profile
    {
        public CustomMapperProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<DemoEntity, DemoDto>().ReverseMap();

            CreateMap<DemoEntity, Demo>().ReverseMap();
        }
    }
}
