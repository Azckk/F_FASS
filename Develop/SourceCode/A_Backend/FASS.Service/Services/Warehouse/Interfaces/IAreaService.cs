using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;

namespace FASS.Service.Services.Warehouse.Interfaces
{
    public interface IAreaService : IAuditService<FrameContext, AreaEntity, AreaDto>
    {
        Task<IPage<StorageDto>> StorageGetPageAsync(string? keyValue, Page page);
    }
}
