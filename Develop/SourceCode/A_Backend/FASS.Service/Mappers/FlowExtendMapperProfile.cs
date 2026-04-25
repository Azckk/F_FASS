using AutoMapper;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Entities.FlowExtend;
using FASS.Service.Models.FlowExtend;

namespace FASS.Service.Mappers
{
    public class FlowExtendMapperProfile : Profile
    {
        public FlowExtendMapperProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<TaskTemplateRuleEntity, TaskTemplateRuleDto>()
                .ForMember(dest => dest.TaskTemplateCode, opt => opt.MapFrom(src => src.TaskTemplateMdcs.Code));
            CreateMap<TaskTemplateRuleDto, TaskTemplateRuleEntity>();
            CreateMap<TaskTemplateMdcsEntity, TaskTemplateMdcsDto>().ReverseMap();
            CreateMap<TaskRecordEntity, TaskRecordDto>().ReverseMap();
            CreateMap<LogisticsRouteEntity, LogisticsRouteDto>()
                .ForMember(dest => dest.SrcAreaCode, opt => opt.MapFrom(src => src.SrcArea.Code))
                .ForMember(dest => dest.DestAreaCode, opt => opt.MapFrom(src => src.DestArea.Code));
            CreateMap<LogisticsRouteDto, LogisticsRouteEntity>();

            CreateMap<TaskTemplateRuleEntity, TaskTemplateRule>();
            CreateMap<TaskTemplateRule, TaskTemplateRuleEntity>()
                .ForMember(dest => dest.TaskTemplateMdcs, opt => opt.Ignore());
            CreateMap<TaskTemplateMdcsEntity, TaskTemplateMdcs>();
            CreateMap<TaskTemplateMdcs, TaskTemplateMdcsEntity>()
                .ForMember(dest => dest.TaskTemplateRules, opt => opt.Ignore());
            CreateMap<TaskRecordEntity, TaskRecord>().ReverseMap();
            CreateMap<LogisticsRouteEntity, LogisticsRoute>();
            CreateMap<LogisticsRoute, LogisticsRouteEntity>()
                .ForMember(dest => dest.SrcArea, opt => opt.Ignore())
                .ForMember(dest => dest.DestArea, opt => opt.Ignore());
        }
    }
}
