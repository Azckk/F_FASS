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
    public class PreMaterialService : AuditService<FrameContext, PreMaterialEntity, PreMaterialDto>, IPreMaterialService
    {
        public PreMaterialService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, PreMaterialEntity> repository,
            IMapper mapper,
            IValidator<PreMaterialDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

    }
}
