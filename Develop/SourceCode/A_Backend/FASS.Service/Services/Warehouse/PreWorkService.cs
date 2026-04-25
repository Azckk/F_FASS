using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.Warehouse;
using FASS.Service.Entities.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.Warehouse
{
    public class PreWorkService : AuditService<FrameContext, PreWorkEntity, PreWorkDto>, IPreWorkService
    {
        public PreWorkService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, PreWorkEntity> repository,
            IMapper mapper,
            IValidator<PreWorkDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {

        }
    }
}
