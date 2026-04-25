using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;

namespace FASS.Service.Services.Warehouse.Interfaces
{
    public interface IMaterialService : IAuditService<FrameContext, MaterialEntity, MaterialDto>
    {
        Task<IPage<MaterialDto>> SelectGetPageAsync(Page page);
        Task<IPage<StorageDto>> StorageGetPageAsync(string keyValue, Page page);
        Task<IPage<MaterialDto>> SelectGetPageIsNoLockAsync(Page page);
        Task<int> StorageAddAsync(string keyValue, IEnumerable<string> storageIds);
        Task<int> StorageAddAsync(string keyValue, IEnumerable<StorageDto> storageDtos);
        Task<int> StorageDeleteAsync(string keyValue, IEnumerable<string> storageIds);
        Task<int> StorageDeleteAsync(string keyValue, IEnumerable<StorageDto> storageDtos);
        IEnumerable<StorageDto> GetStorages(MaterialDto materialDto);
        IEnumerable<StorageDto> GetStorages(IEnumerable<MaterialDto> materialDtos);
        Task<IPage<ContainerDto>> ContainerGetPageAsync(string keyValue, Page page);
        Task<int> ContainerAddAsync(string keyValue, IEnumerable<string> containerIds);
        Task<int> ContainerAddAsync(string keyValue, IEnumerable<ContainerDto> containerDtos);
        Task<int> ContainerDeleteAsync(string keyValue, IEnumerable<string> containerIds);
        Task<int> ContainerDeleteAsync(string keyValue, IEnumerable<ContainerDto> containerDtos);
        IEnumerable<ContainerDto> GetContainers(MaterialDto materialDto);
        IEnumerable<ContainerDto> GetContainers(IEnumerable<MaterialDto> materialDtos);
        Task<IEnumerable<ContainerDto>> GetContainersAsync(MaterialDto materialDto);
        Task<IEnumerable<ContainerDto>> GetContainersAsync(IEnumerable<MaterialDto> materialDtos);
        Task<IEnumerable<StorageDto>> GetStoragesByMaterialTypeAsync(string area, string materialType, bool isLock);

    }
}
