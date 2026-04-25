using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.Warehouse
{
    public class StorageContainerHistoryService : AuditService<FrameContext, StorageContainerHistoryEntity, StorageContainerHistoryDto>, IStorageContainerHistoryService
    {
        public StorageContainerHistoryService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, StorageContainerHistoryEntity> repository,
            IMapper mapper,
            IValidator<StorageContainerHistoryDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

        public async Task<int> DeleteM3Async()
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddMonths(-3));
        }

        public async Task<int> DeleteM1Async()
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddMonths(-1));
        }

        public async Task<int> DeleteW1Async()
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddDays(-7));
        }

        public async Task<int> DeleteD1Async()
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddDays(-1));
        }

        public async Task<int> DeleteAllAsync()
        {
            return await Repository.ExecuteDeleteAsync(e => true);
        }

        public async Task<int> DeleteDayAsync(int day = 90)
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddDays(-day));
        }

    }
}
