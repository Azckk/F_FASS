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
    public class StorageTagService : AuditService<FrameContext, StorageTagEntity, StorageTagDto>, IStorageTagService
    {
        public StorageTagService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, StorageTagEntity> repository,
            IMapper mapper,
            IValidator<StorageTagDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }
    }
}
