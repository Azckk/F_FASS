using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;

namespace FASS.Service.Services.Warehouse.Interfaces
{
    public interface IMaterialStorageHistoryService : IAuditService<FrameContext, MaterialStorageHistoryEntity, MaterialStorageHistoryDto>
    {
        Task<int> DeleteM3Async();
        Task<int> DeleteM1Async();
        Task<int> DeleteW1Async();
        Task<int> DeleteD1Async();
        Task<int> DeleteAllAsync();
        Task<int> DeleteDayAsync(int day = 90);

    }
}
