using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Dtos.FlowExtend;

namespace FASS.Service.Services.Warehouse.Interfaces
{
    public interface IContainerService : IAuditService<FrameContext, ContainerEntity, ContainerDto>
    {
        new Task<int> AddOrUpdateAsync(string? keyValue, ContainerDto containerDto);
        Task<IPage<ContainerDto>> SelectGetPageAsync(Page page);
        Task<IPage<MaterialDto>> MaterialGetPageAsync(string keyValue, Page page);
        Task<IEnumerable<ContainerMaterialDto>> GetContainerMaterialListAsync(string keyValue);
        Task<int> MaterialAddAsync(string keyValue, IEnumerable<string> materialIds);
        Task<int> MaterialAddAsync(string keyValue, IEnumerable<MaterialDto> materialDtos);
        Task<int> MaterialDeleteAsync(string keyValue, IEnumerable<string> materialIds);
        Task<int> MaterialDeleteAsync(string keyValue, IEnumerable<MaterialDto> materialDtos);
        Task<int> MaterialDeleteAsync(string keyValue);
        int MaterialDelete(string keyValue);
        Task<IEnumerable<MaterialDto>> GetMaterialsAsync(ContainerDto containerDto);
        Task<IEnumerable<MaterialDto>> GetMaterialsAsync(IEnumerable<ContainerDto> containerDtos);
        IEnumerable<MaterialDto> GetMaterials(ContainerDto containerDto);
        IEnumerable<MaterialDto> GetMaterials(string keyValue);
        IEnumerable<MaterialDto> GetMaterials(IEnumerable<ContainerDto> containerDtos);
        Task<IPage<StorageDto>> StorageGetPageAsync(string keyValue, Page page);
        Task<int> StorageAddAsync(string keyValue, IEnumerable<string> storageIds);
        Task<int> StorageAddAsync(string keyValue, IEnumerable<StorageDto> storageDtos);
        Task<int> StorageDeleteAsync(string keyValue, IEnumerable<string> storageIds);
        Task<int> StorageDeleteAsync(string keyValue, IEnumerable<StorageDto> storageDtos);
        IEnumerable<StorageDto> GetStorages(ContainerDto containerDto);
        IEnumerable<StorageDto> GetStorages(IEnumerable<ContainerDto> containerDtos);
        Task<IEnumerable<StorageDto>> GetStoragesAsync(ContainerDto containerDto);
        Task<IEnumerable<StorageDto>> GetStoragesAsync(IEnumerable<ContainerDto> containerDtos);
    }
}
