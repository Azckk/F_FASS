using AutoMapper;
using FASS.Service.Dtos.Setting;
using FASS.Service.Entities.Setting;


namespace FASS.Service.Mappers
{
    public class SetExtendMapper : Profile
    {
        public SetExtendMapper()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<ConfigChargeEntity, ConfigChargeDto>().ReverseMap();

         /*   CreateMap<ConfigEnvelopeEntity, ConfigEnvelopeDto>().ReverseMap();

            CreateMap<ConfigTrafficControlEntity, ConfigTrafficControlDto>().ReverseMap();*/

        }
    }
}
