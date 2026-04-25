using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.Warehouse;
using FASS.Service.Entities.Warehouse;

namespace FASS.Service.Services.Warehouse.Interfaces
{
    public interface IStorageTagService : IAuditService<FrameContext, StorageTagEntity, StorageTagDto>
    {

    }
}
