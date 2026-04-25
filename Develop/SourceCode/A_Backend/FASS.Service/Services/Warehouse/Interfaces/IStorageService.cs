using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Dtos.Warehouse;

namespace FASS.Service.Services.Warehouse.Interfaces
{
    public interface IStorageService : IAuditService<FrameContext, StorageEntity, StorageDto>
    {
        new Task<int> AddOrUpdateAsync(string? keyValue, StorageDto storageDto);
        Task<IPage<StorageDto>> SelectGetPageAsync(Page page);
        Task<IPage<ContainerDto>> ContainerGetPageAsync(string keyValue, Page page);
        Task<IEnumerable<StorageContainerDto>> GetStorageContainerListAsync(string keyValue);
        Task<int> ContainerAddAsync(string keyValue, IEnumerable<string> containerIds);
        Task<int> ContainerAddAsync(string keyValue, IEnumerable<ContainerDto> containerDtos);
        Task<int> ContainerDeleteAsync(string keyValue, IEnumerable<string> containerIds);
        Task<int> ContainerDeleteAsync(string keyValue, IEnumerable<ContainerDto> containerDtos);
        Task<IEnumerable<ContainerDto>> GetContainersAsync(StorageDto storageDto);
        Task<IEnumerable<ContainerDto>> GetContainersAsync(IEnumerable<StorageDto> storageDtos);
        IEnumerable<ContainerDto> GetContainers(StorageDto storageDto);
        IEnumerable<ContainerDto> GetContainers(IEnumerable<StorageDto> storageDtos);
        Task<IPage<MaterialDto>> MaterialGetPageAsync(string keyValue, Page page);
        Task<int> MaterialAddAsync(string keyValue, IEnumerable<string> materialIds);
        Task<int> MaterialAddAsync(string keyValue, IEnumerable<MaterialDto> materialDtos);
        Task<int> MaterialDeleteAsync(string keyValue, IEnumerable<string> materialIds);
        Task<int> MaterialDeleteAsync(string keyValue, IEnumerable<MaterialDto> materialDtos);
        IEnumerable<MaterialDto> GetMaterials(StorageDto storageDto);
        IEnumerable<MaterialDto> GetMaterials(IEnumerable<StorageDto> storageDtos);

        Task<IPage<TagDto>> TagGetPageAsync(string keyValue, Page page);
        Task<int> TagAddAsync(string keyValue, IEnumerable<TagDto> tagDtos);
        Task<int> TagAddAsync(string keyValue, IEnumerable<string> tagIds);
        Task<int> TagUpdateAsync(string keyValue, IEnumerable<TagDto> tagDtos);
        Task<int> TagUpdateAsync(string keyValue, IEnumerable<string> tagIds);
        Task<int> TagDeleteAsync(string keyValue, IEnumerable<string> tagIds);
        Task<int> TagDeleteAsync(string keyValue, IEnumerable<TagDto> tagDtos);
        Task<IEnumerable<TagDto>> GetTagsAsync(StorageDto storageDto);
        IEnumerable<TagDto> GetTags(IEnumerable<StorageDto> storageDtos);
        int UpdateStorageState(StorageDto storageDto);
        int ContainerAdd(string keyValue, IEnumerable<string> containerIds);
        int ContainerAdd(string keyValue, IEnumerable<ContainerDto> containerDtos);
        int ContainerDelete(string keyValue, IEnumerable<string> containerIds);
        int ContainerDelete(string keyValue, IEnumerable<ContainerDto> containerDtos);
        int TagAdd(string keyValue, IEnumerable<TagDto> tagDtos);
        int TagAdd(string keyValue, IEnumerable<string> tagIds);
        int TagDelete(string keyValue, IEnumerable<TagDto> tagDtos);
        int TagDelete(string keyValue, IEnumerable<string> tagIds);
    }
}
