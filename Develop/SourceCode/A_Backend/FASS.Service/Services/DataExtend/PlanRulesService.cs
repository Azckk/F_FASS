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
    public class PlanRulesService : AuditService<FrameContext, PlanRulesEntity, PlanRulesDto>, IPlanRulesService
    {
        public PlanRulesService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, PlanRulesEntity> repository,
            IMapper mapper,
            IValidator<PlanRulesDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }
    }
}
