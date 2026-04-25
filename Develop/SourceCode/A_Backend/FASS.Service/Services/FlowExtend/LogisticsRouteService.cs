using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Entities.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.FlowExtend
{
    public class LogisticsRouteService : AuditService<FrameContext, LogisticsRouteEntity, LogisticsRouteDto>, ILogisticsRouteService
    {
        public LogisticsRouteService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, LogisticsRouteEntity> repository,
            IMapper mapper,
            IValidator<LogisticsRouteDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

    }
}
