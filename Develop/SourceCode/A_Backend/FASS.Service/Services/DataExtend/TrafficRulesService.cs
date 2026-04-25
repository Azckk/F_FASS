using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Entities.DataExtend;
using FASS.Service.Services.DataExtend.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.DataExtend
{
    public class TrafficRulesService : AuditService<FrameContext, TrafficRulesEntity, TrafficRulesDto>, ITrafficRulesService
    {
        public TrafficRulesService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, TrafficRulesEntity> repository,
            IMapper mapper,
            IValidator<TrafficRulesDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }
    }
}
