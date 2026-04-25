using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Dtos.FlowExtend;

namespace FASS.Service.Services.Warehouse.Interfaces
{
    public interface IWorkService : IAuditService<FrameContext, WorkEntity, WorkDto>
    {
        Task<int> DeleteM3Async(string? type = null);
        Task<int> DeleteM1Async(string? type = null);
        Task<int> DeleteW1Async(string? type = null);
        Task<int> DeleteD1Async(string? type = null);
        Task<int> DeleteAllAsync(string? type = null);
        Task<int> DeleteDayAsync(string? type = null, int day = 90);
        Task<int> AddWorkAsync(string areaId, string callModeName, TaskRecordDto taskRecordDto, ContainerDto containerDto, string material, string workType = "");
        Task<int> UpdateWorkStateAsync(string keyValue, string state);
    }
}