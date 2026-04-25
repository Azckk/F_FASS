using AutoMapper;
using FASS.Service.Dtos.BaseExtend;
using FASS.Service.Entities.BaseExtend;
using FASS.Service.Models.BaseExtend;

namespace FASS.Service.Mappers
{
    public class BaseExtendMapperProfile : Profile
    {
        public BaseExtendMapperProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<NodeAttributeEntity, NodeAttributeDto>().ReverseMap();
            CreateMap<EdgeAttributeEntity, EdgeAttributeDto>().ReverseMap();
            CreateMap<ZoneAttributeEntity, ZoneAttributeDto>().ReverseMap();
            CreateMap<ZonePlanRulesEntity, ZonePlanRulesDto>().ReverseMap();
            CreateMap<NodePlanRulesEntity, NodePlanRulesDto>().ReverseMap();
            CreateMap<NodeMustFreeEntity, NodeMustFreeDto>().ReverseMap();

            CreateMap<NodeAttributeEntity, NodeAttribute>();
            CreateMap<EdgeAttributeEntity, EdgeAttribute>();
            CreateMap<ZoneAttributeEntity, ZoneAttribute>();
            CreateMap<ZonePlanRulesEntity, ZonePlanRules>();
            CreateMap<NodePlanRulesEntity, NodePlanRules>();
            CreateMap<NodeMustFreeEntity, NodeMustFree>();
        }
    }
}
