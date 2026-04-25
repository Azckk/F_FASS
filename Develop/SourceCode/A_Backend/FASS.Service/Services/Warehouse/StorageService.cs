using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.NETCore;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Service;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Dtos.Warehouse;
using FASS.Service.Entities.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FASS.Service.Services.Warehouse
{
    public class StorageService
        : AuditService<FrameContext, StorageEntity, StorageDto>,
            IStorageService
    {
        public StorageService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, StorageEntity> repository,
            IMapper mapper,
            IValidator<StorageDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }


        public override async Task<int> AddOrUpdateAsync(string? keyValue, StorageDto storageDto)
        {
            // 初始化结果变量
            var result = 0;

            try
            {
                var storageEntity = Mapper.Map<StorageEntity>(storageDto);
                // 开始事务
                await UnitOfWork.BeginAsync();

                // 获取相关仓储库
                var containerMaterialRepository = UnitOfWork.GetRepository<ContainerMaterialEntity>();
                var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                var storageRepo = UnitOfWork.GetRepository<StorageEntity>();
                var containerRepo = UnitOfWork.GetRepository<ContainerEntity>();

                // 如果 keyValue 为空或 null，则添加新库位
                if (string.IsNullOrEmpty(keyValue))
                {
                    await storageRepo.AddAsync(storageEntity);
                    result = 1;
                }
                else
                {
                    // 获取库位与容器的绑定情况
                    var storageContainer = await storageContainerRepo.FirstOrDefaultAsync(e => e.StorageId == keyValue);
                    // 获取当前库位信息
                    var storage = Guard.NotNull(await storageRepo.FirstOrDefaultAsync(e => e.Id == keyValue));

                    if (storageContainer != null)
                    {
                        // 获取容器信息
                        var container = await containerRepo.FirstOrDefaultAsync(e => e.Id == storageContainer.ContainerId);

                        if (IsInvalidStateTransition(storage, container, storageDto))
                        {
                            result = 0;
                        }
                        else
                        {
                            // 更新库位状态
                            storageEntity.Id = storage.Id;
                            await storageRepo.UpdateAsync(storageEntity);
                            result = 1;
                        }
                    }
                    else
                    {
                        // 更新库位状态
                        storageEntity.Id = storage.Id;
                        await storageRepo.UpdateAsync(storageEntity);
                        result = 1;
                    }
                }

                // 提交事务
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                // 发生异常时回滚事务
                await UnitOfWork.RollbackAsync();
                throw;
            }

            return result;
        }

        private bool IsInvalidStateTransition(StorageEntity storage, ContainerEntity? container, StorageDto storageDto)
        {
            return storage != null && container != null &&
                   ((storage.State == StorageConst.State.EmptyContainer && container.State == ContainerConst.State.EmptyMaterial && storageDto.State == StorageConst.State.FullContainer) ||
                    (storage.State == StorageConst.State.FullContainer && container.State == ContainerConst.State.FullMaterial && storageDto.State == StorageConst.State.EmptyContainer) ||
                    storage.State == StorageConst.State.NoneContainer);
        }


        public async Task<IPage<StorageDto>> SelectGetPageAsync(Page page)
        {
            var result = await Repository
                .Set()
                .ProjectTo<StorageDto>(Mapper.ConfigurationProvider)
                .Where(e => e.IsEnable)
                .ToPageAsync(page);
            return result;
        }

        public async Task<IPage<ContainerDto>> ContainerGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Container)
                .ProjectTo<ContainerDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }

        public async Task<IEnumerable<StorageContainerDto>> GetStorageContainerListAsync(
            string keyValue
        )
        {
            var dto = await Repository
                .Set()
                .AsNoTracking()
                .Where(e =>
                    string.IsNullOrEmpty(keyValue) ? e.IsEnable : (e.Id == keyValue && e.IsEnable)
                )
                .SelectMany(e => e.StorageContainers)
                .ProjectTo<StorageContainerDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return dto;
        }

        #region 添加容器老版
        /* public async Task<int> ContainerAddAsync(string keyValue, IEnumerable<string> containerIds)
         {
             var result = 0;
             try
             {
                 await UnitOfWork.BeginAsync();
                 foreach (var containerId in containerIds)
                 {
                     var entity = new StorageContainerEntity() { StorageId = keyValue, ContainerId = containerId };
                     var isExist = await UnitOfWork.GetRepository<StorageContainerEntity>().AnyAsync(e => e.StorageId == entity.StorageId && e.ContainerId == entity.ContainerId);
                     if (!isExist)
                     {
                         //更行容器状态
                       var container=  await UnitOfWork.GetRepository<ContainerEntity>().FirstOrDefaultAsync((e => e.Id == containerId));
                         if (container!=null)
                         {
                             container.IsLock = true;
                             await UnitOfWork.GetRepository<ContainerEntity>().UpdateAsync(container);
                         }
                         await UnitOfWork.GetRepository<StorageContainerEntity>().AddAsync(entity);
                         await UnitOfWork.GetRepository<StorageContainerHistoryEntity>().AddAsync(new StorageContainerHistoryEntity()
                         {
                             StorageId = entity.StorageId,
                             ContainerId = entity.ContainerId,
                             State = StorageContainerHistoryConst.State.Add
                         });
                         result++;
                     }
                 }
                 await UnitOfWork.CommitAsync();
             }
             catch
             {
                 await UnitOfWork.RollbackAsync();
                 throw;
             }
             return result;
         }*/
        #endregion

        public async Task<int> ContainerAddAsync(string keyValue, IEnumerable<string> containerIds)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                throw new ArgumentException("Key value cannot be null or empty.", nameof(keyValue));
            }

            if (containerIds == null || !containerIds.Any())
            {
                throw new ArgumentException("Container IDs cannot be null or empty.", nameof(containerIds));
            }

            var result = 0;

            try
            {
                await UnitOfWork.BeginAsync();

                var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                var containerRepo = UnitOfWork.GetRepository<ContainerEntity>();
                var storageContainerHistoryRepo = UnitOfWork.GetRepository<StorageContainerHistoryEntity>();
                var storageRepo = UnitOfWork.GetRepository<StorageEntity>();

                var entitiesToAdd = new List<StorageContainerEntity>();
                var entitiesToUpdate = new List<StorageContainerEntity>();
                var historyEntitiesToAdd = new List<StorageContainerHistoryEntity>();
                var containersToUpdate = new List<ContainerEntity>();

                // 查询现有的 StorageContainerEntity
                var existingEntity = await storageContainerRepo.Set()
                    .FirstOrDefaultAsync(e => e.StorageId == keyValue);

                // 批量查询所有 ContainerEntity
                var containers = await containerRepo.Set()
                    .Where(c => containerIds.Contains(c.Id))
                    .ToDictionaryAsync(c => c.Id);

                var storageEntity = await storageRepo.FirstOrDefaultAsync(e => e.Id == keyValue);
                //如果没有添加的容器且库位现在没有绑定的容器 则库位状态为无容器状态
                if (!containerIds.Any() && existingEntity == null)
                {
                    if(storageEntity is not null)
                    storageEntity.State = StorageConst.State.NoneContainer;
                }

                // 获取当前绑定的容器的对象
                ContainerEntity? currentContainer = null;
                if (existingEntity != null)
                {
                    containers.TryGetValue(existingEntity.ContainerId, out currentContainer);
                }
                //遍历所有传递进来的容器ID
                foreach (var containerId in containerIds)
                {
                    //创建容器和库位绑定关系实体
                    var entity = new StorageContainerEntity
                    {
                        StorageId = keyValue,
                        ContainerId = containerId
                    };
                    //获取容器实体
                    containers.TryGetValue(containerId, out var container);

                    //如果库位没有绑定容器
                    if (existingEntity == null)
                    {

                        if (container != null)
                        {
                            container.IsLock = true;
                            //修改容器状态为锁定状态
                            containersToUpdate.Add(container);
                        }

                        // 根据容器状态修改库位状态
                        if (storageEntity != null && container != null)
                        {
                            UpdateStorageEntityState(storageEntity, container);
                        }
                        //添加容器和库位关系
                        entitiesToAdd.Add(entity);
                        //创建容器和库位关系历史记录
                        historyEntitiesToAdd.Add(CreateHistoryEntity(entity, StorageContainerHistoryConst.State.Add));
                        result++;
                    }
                    else
                    {
                        if (existingEntity.ContainerId != containerId)
                        {
                            historyEntitiesToAdd.Add(CreateHistoryEntity(entity, StorageContainerHistoryConst.State.Add));
                            existingEntity.ContainerId = containerId;
                            //从新绑定容器
                            if (container != null)
                            {
                                container.IsLock = true;
                                containersToUpdate.Add(container);
                            }
                            //当前绑定的容器状态释放
                            if (currentContainer != null)
                            {
                                currentContainer.IsLock = false;
                                containersToUpdate.Add(currentContainer);
                            }

                            // 根据容器状态修改库位状态
                            if (storageEntity != null && container != null)
                            {
                                UpdateStorageEntityState(storageEntity, container);
                            }

                            // 修改库位容器关系
                            entitiesToUpdate.Add(existingEntity);
                        }
                    }
                }

                if (entitiesToAdd.Any())
                {
                    await storageContainerRepo.AddAsync(entitiesToAdd);
                }

                if (entitiesToUpdate.Any())
                {
                    await storageContainerRepo.UpdateAsync(entitiesToUpdate);
                }

                if (historyEntitiesToAdd.Any())
                {
                    await storageContainerHistoryRepo.AddAsync(historyEntitiesToAdd);
                }

                if (containersToUpdate.Any())
                {
                    await containerRepo.UpdateAsync(containersToUpdate);
                }

                if (storageEntity is not null)
                {
                    await storageRepo.UpdateAsync(storageEntity);
                }

                await UnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"storage add container error {ex}");
                await UnitOfWork.RollbackAsync();
                throw;
            }

            return result;
        }

        private static void UpdateStorageEntityState(StorageEntity storageEntity, ContainerEntity container)
        {
            if (container == null) return;

            switch (container.State)
            {
                case ContainerConst.State.EmptyMaterial:
                    storageEntity.State = StorageConst.State.EmptyContainer;
                    break;
                case ContainerConst.State.FullMaterial:
                    storageEntity.State = StorageConst.State.FullContainer;
                    break;
                default:
                    storageEntity.State = StorageConst.State.NoneContainer;
                    break;
            }
        }

        private static StorageContainerHistoryEntity CreateHistoryEntity(StorageContainerEntity entity, string state)
        {
            return new StorageContainerHistoryEntity
            {
                StorageId = entity.StorageId,
                ContainerId = entity.ContainerId,
                State = state
            };
        }
        public async Task<int> ContainerAddAsync(
            string keyValue,
            IEnumerable<ContainerDto> containerDtos
        )
        {
            var containerIds = containerDtos.Select(e => e.Id);
            return await ContainerAddAsync(keyValue, containerIds);
        }
        #region 删除库位与容器关系老代码
        /*  public async Task<int> ContainerDeleteAsync(
              string keyValue,
              IEnumerable<string> containerIds
          )
          {
              var result = 0;
              try
              {
                  await UnitOfWork.BeginAsync();
                  var containerRepo = UnitOfWork.GetRepository<ContainerEntity>();
                  // 批量查询所有 ContainerEntity
                  var containers = await containerRepo.Set()
                      .Where(c => containerIds.Contains(c.Id))
                      .ToDictionaryAsync(c => c.Id);
                  foreach (var containerId in containerIds)
                  {
                      var entity = new StorageContainerEntity()
                      {
                          StorageId = keyValue,
                          ContainerId = containerId
                      };
                      var isExist = await UnitOfWork
                          .GetRepository<StorageContainerEntity>()
                          .AnyAsync(e =>
                              e.StorageId == entity.StorageId && e.ContainerId == entity.ContainerId
                          );
                      if (isExist)
                      {
                          await UnitOfWork
                              .GetRepository<StorageContainerEntity>()
                              .DeleteAsync(e =>
                                  e.StorageId == entity.StorageId
                                  && e.ContainerId == entity.ContainerId
                              );
                          await UnitOfWork
                              .GetRepository<StorageContainerHistoryEntity>()
                              .AddAsync(
                                  new StorageContainerHistoryEntity()
                                  {
                                      StorageId = entity.StorageId,
                                      ContainerId = entity.ContainerId,
                                      State = StorageContainerHistoryConst.State.Delete
                                  }
                              );
                          //修改容器状态为未锁定
                          containers.TryGetValue(containerId, out var container);
                          container.IsLock = false;
                          await containerRepo.UpdateAsync(container);
                          result++;
                      }
                  }
                  await UnitOfWork.CommitAsync();
              }
              catch
              {
                  await UnitOfWork.RollbackAsync();
                  throw;
              }
              return result;
          }*/
        #endregion

        public async Task<int> ContainerDeleteAsync(string keyValue, IEnumerable<string> containerIds)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                throw new ArgumentException("Key value cannot be null or empty.", nameof(keyValue));
            }

            if (containerIds == null || !containerIds.Any())
            {
                throw new ArgumentException("Container IDs cannot be null or empty.", nameof(containerIds));
            }

            var result = 0;

            try
            {
                await UnitOfWork.BeginAsync();
                var storageRepo = UnitOfWork.GetRepository<StorageEntity>();
                var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                var containerRepo = UnitOfWork.GetRepository<ContainerEntity>();
                var storageContainerHistoryRepo = UnitOfWork.GetRepository<StorageContainerHistoryEntity>();

                // 批量查询所有 ContainerEntity 和 StorageContainerEntity
                var containers = await containerRepo.Set()
                    .Where(c => containerIds.Contains(c.Id))
                    .ToDictionaryAsync(c => c.Id);

                var existingEntities = await storageContainerRepo.Set()
                    .Where(e => e.StorageId == keyValue && containerIds.Contains(e.ContainerId))
                    .ToListAsync();
                var storageEntity = await storageRepo.FirstOrDefaultAsync(e => e.Id == keyValue);
                //若果没有找到库位和容器绑定的关系
                if (storageEntity == null)
                {
                    throw new ArgumentException("storageEntity is not found.", nameof(keyValue));
                }
                var entitiesToDelete = new List<StorageContainerEntity>();
                var historyEntitiesToAdd = new List<StorageContainerHistoryEntity>();
                var containersToUpdate = new List<ContainerEntity>();
                //遍历所有需要删除的容器
                foreach (var containerId in containerIds)
                {
                    //获取需要删除的容器和库位关系实体
                    var existingEntity = existingEntities.FirstOrDefault(e => e.ContainerId == containerId);

                    if (existingEntity != null)
                    {
                        entitiesToDelete.Add(existingEntity);
                        //添加库位和容器的关系历史记录
                        historyEntitiesToAdd.Add(new StorageContainerHistoryEntity
                        {
                            StorageId = existingEntity.StorageId,
                            ContainerId = existingEntity.ContainerId,
                            State = StorageContainerHistoryConst.State.Delete
                        });

                        if (containers.TryGetValue(containerId, out var container))
                        {
                            container.IsLock = false;
                            containersToUpdate.Add(container);
                        }

                        result++;
                    }
                }

                if (entitiesToDelete.Any())
                {
                    await storageContainerRepo.DeleteAsync(entitiesToDelete);
                }

                if (historyEntitiesToAdd.Any())
                {
                    await storageContainerHistoryRepo.AddAsync(historyEntitiesToAdd);
                }

                if (containersToUpdate.Any())
                {
                    await containerRepo.UpdateAsync(containersToUpdate);
                }
                storageEntity.State = StorageConst.State.NoneContainer;
                await storageRepo.UpdateAsync(storageEntity);
                await UnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"storage deleting containers error .{ex}");
                await UnitOfWork.RollbackAsync();
                // 记录异常日志
                // Logger.LogError(ex, "Error occurred while deleting containers.");
                throw;
            }

            return result;
        }
        public async Task<int> ContainerDeleteAsync(
            string keyValue,
            IEnumerable<ContainerDto> containerDtos
        )
        {
            var containerIds = containerDtos.Select(e => e.Id);
            return await ContainerDeleteAsync(keyValue, containerIds);
        }

        public IEnumerable<ContainerDto> GetContainers(StorageDto storageDto)
        {
            var storageEntity = Mapper.Map<StorageEntity>(storageDto);
            var containerEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => e == storageEntity)
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Container)
                .ToList();
            var containerDtos = Mapper.Map<IEnumerable<ContainerDto>>(containerEntities);
            return containerDtos;
        }

        public async Task<IEnumerable<ContainerDto>> GetContainersAsync(StorageDto storageDto)
        {
            var storageEntity = Mapper.Map<StorageEntity>(storageDto);
            var containerDtos = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e == storageEntity)
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Container)
                .ProjectTo<ContainerDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return containerDtos;
        }

        public async Task<IEnumerable<ContainerDto>> GetContainersAsync(
            IEnumerable<StorageDto> storageDtos
        )
        {
            var storageEntities = Mapper.Map<IEnumerable<StorageEntity>>(storageDtos);
            var containerEntities = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => storageEntities.Contains(e))
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Container)
                .ToListAsync();
            var containerDtos = Mapper.Map<IEnumerable<ContainerDto>>(containerEntities);
            return containerDtos;
        }

        public IEnumerable<ContainerDto> GetContainers(IEnumerable<StorageDto> storageDtos)
        {
            var storageEntities = Mapper.Map<IEnumerable<StorageEntity>>(storageDtos);
            var containerEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => storageEntities.Contains(e))
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Container)
                .ToList();
            var containerDtos = Mapper.Map<IEnumerable<ContainerDto>>(containerEntities);
            return containerDtos;
        }

        public async Task<IPage<MaterialDto>> MaterialGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.MaterialStorages)
                .Select(e => e.Material)
                .ProjectTo<MaterialDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }

        public async Task<int> MaterialAddAsync(string keyValue, IEnumerable<string> materialIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();

                foreach (var materialId in materialIds)
                {
                    var entity = new MaterialStorageEntity()
                    {
                        MaterialId = materialId,
                        StorageId = keyValue
                    };
                    var isExist = await UnitOfWork
                        .GetRepository<MaterialStorageEntity>()
                        .AnyAsync(e =>
                            e.MaterialId == entity.MaterialId && e.StorageId == entity.StorageId
                        );
                    if (!isExist)
                    {
                        await UnitOfWork.GetRepository<MaterialStorageEntity>().AddAsync(entity);
                        await UnitOfWork
                            .GetRepository<MaterialStorageHistoryEntity>()
                            .AddAsync(
                                new MaterialStorageHistoryEntity()
                                {
                                    MaterialId = entity.MaterialId,
                                    StorageId = entity.StorageId,
                                    State = MaterialStorageHistoryConst.State.Add
                                }
                            );
                        result++;
                    }
                }
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> MaterialAddAsync(
            string keyValue,
            IEnumerable<MaterialDto> materialDtos
        )
        {
            var materialIds = materialDtos.Select(e => e.Id);
            //根据库位ID获取库位绑定的容器信息
            var storageContainer = await UnitOfWork
                .GetRepository<StorageContainerEntity>()
                .FirstOrDefaultAsync(e => e.StorageId == keyValue);
            if (storageContainer != null)
            {
                return await MaterialAddAsync(keyValue, materialIds);
            }

            return 0;
        }

        private async Task<int> StorageAddMaterialAsync(
            string keyValue,
            IEnumerable<string> materialIds,
            StorageContainerEntity storageContainer
        )
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();

                foreach (var materialId in materialIds)
                {
                    var entity = new MaterialStorageEntity()
                    {
                        MaterialId = materialId,
                        StorageId = keyValue
                    };
                    var isExist = await UnitOfWork
                        .GetRepository<MaterialStorageEntity>()
                        .AnyAsync(e =>
                            e.MaterialId == entity.MaterialId && e.StorageId == entity.StorageId
                        );
                    if (!isExist)
                    {
                        await UnitOfWork.GetRepository<MaterialStorageEntity>().AddAsync(entity);
                        await UnitOfWork
                            .GetRepository<MaterialStorageHistoryEntity>()
                            .AddAsync(
                                new MaterialStorageHistoryEntity()
                                {
                                    MaterialId = entity.MaterialId,
                                    StorageId = entity.StorageId,
                                    State = MaterialStorageHistoryConst.State.Add
                                }
                            );
                        result++;
                    }
                }
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> MaterialDeleteAsync(string keyValue, IEnumerable<string> materialIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                foreach (var materialId in materialIds)
                {
                    var entity = new MaterialStorageEntity()
                    {
                        MaterialId = materialId,
                        StorageId = keyValue
                    };
                    var isExist = await UnitOfWork
                        .GetRepository<MaterialStorageEntity>()
                        .AnyAsync(e =>
                            e.MaterialId == entity.MaterialId && e.StorageId == entity.StorageId
                        );
                    if (isExist)
                    {
                        await UnitOfWork
                            .GetRepository<MaterialStorageEntity>()
                            .DeleteAsync(e =>
                                e.MaterialId == entity.MaterialId && e.StorageId == entity.StorageId
                            );
                        await UnitOfWork
                            .GetRepository<MaterialStorageHistoryEntity>()
                            .AddAsync(
                                new MaterialStorageHistoryEntity()
                                {
                                    MaterialId = entity.MaterialId,
                                    StorageId = entity.StorageId,
                                    State = MaterialStorageHistoryConst.State.Delete
                                }
                            );
                        result++;
                    }
                }
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> MaterialDeleteAsync(
            string keyValue,
            IEnumerable<MaterialDto> materialDtos
        )
        {
            var materialIds = materialDtos.Select(e => e.Id);
            return await MaterialDeleteAsync(keyValue, materialIds);
        }

        public IEnumerable<MaterialDto> GetMaterials(StorageDto storageDto)
        {
            var storageEntity = Mapper.Map<StorageEntity>(storageDto);
            var materialEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => e == storageEntity)
                .SelectMany(e => e.MaterialStorages)
                .Select(e => e.Material)
                .ToList();
            var materialDtos = Mapper.Map<IEnumerable<MaterialDto>>(materialEntities);
            return materialDtos;
        }

        public IEnumerable<MaterialDto> GetMaterials(IEnumerable<StorageDto> storageDtos)
        {
            var storageEntities = Mapper.Map<IEnumerable<StorageEntity>>(storageDtos);
            var materialEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => storageEntities.Contains(e))
                .SelectMany(e => e.MaterialStorages)
                .Select(e => e.Material)
                .ToList();
            var materialDtos = Mapper.Map<IEnumerable<MaterialDto>>(materialEntities);
            return materialDtos;
        }

        public async Task<IPage<TagDto>> TagGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.StorageTags)
                .Select(e => e.Tag)
                .ProjectTo<TagDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }

        public async Task<int> TagAddAsync(string keyValue, IEnumerable<TagDto> tagDtos)
        {
            var tagIds = tagDtos.Select(e => e.Id);
            return await TagAddAsync(keyValue, tagIds);
        }

        public async Task<int> TagAddAsync(string keyValue, IEnumerable<string> tagIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                foreach (var tagId in tagIds)
                {
                    var entity = new StorageTagEntity() { TagId = tagId, StorageId = keyValue };
                    var isExist = await UnitOfWork
                        .GetRepository<StorageTagEntity>()
                        .AnyAsync(e => e.TagId == entity.TagId && e.StorageId == entity.StorageId);
                    if (!isExist)
                    {
                        await UnitOfWork.GetRepository<StorageTagEntity>().AddAsync(entity);
                        result++;
                    }
                }
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> TagUpdateAsync(string keyValue, IEnumerable<TagDto> tagDtos)
        {
            var tagIds = tagDtos.Select(e => e.Id);
            return await TagUpdateAsync(keyValue, tagIds);
        }

        public async Task<int> TagUpdateAsync(string keyValue, IEnumerable<string> tagIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                //清理库位id对应的标签信息
                await UnitOfWork
                    .GetRepository<StorageTagEntity>()
                    .DeleteAsync(e => e.StorageId == keyValue);
                //添加库位标签对应关系
                foreach (var tagId in tagIds)
                {
                    var entity = new StorageTagEntity() { TagId = tagId, StorageId = keyValue };
                    await UnitOfWork.GetRepository<StorageTagEntity>().AddAsync(entity);
                    result++;
                }
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> TagDeleteAsync(string keyValue, IEnumerable<string> tagIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                foreach (var tagId in tagIds)
                {
                    var entity = new StorageTagEntity() { TagId = tagId, StorageId = keyValue };
                    var isExist = await UnitOfWork
                        .GetRepository<StorageTagEntity>()
                        .AnyAsync(e => e.TagId == entity.TagId && e.StorageId == entity.StorageId);
                    if (isExist)
                    {
                        await UnitOfWork
                            .GetRepository<StorageTagEntity>()
                            .DeleteAsync(e =>
                                e.TagId == entity.TagId && e.StorageId == entity.StorageId
                            );
                        result++;
                    }
                }
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> TagDeleteAsync(string keyValue, IEnumerable<TagDto> tagDtos)
        {
            var tagIds = tagDtos.Select(e => e.Id);
            return await TagDeleteAsync(keyValue, tagIds);
        }

        public async Task<IEnumerable<TagDto>> GetTagsAsync(StorageDto storageDto)
        {
            var storageEntity = Mapper.Map<StorageEntity>(storageDto);
            var tagEntities = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e == storageEntity)
                .SelectMany(e => e.StorageTags)
                .Select(e => e.Tag)
                .ToListAsync();
            var tagDtos = Mapper.Map<IEnumerable<TagDto>>(tagEntities);
            return tagDtos;
        }

        public IEnumerable<TagDto> GetTags(IEnumerable<StorageDto> storageDtos)
        {
            var storageEntities = Mapper.Map<IEnumerable<StorageEntity>>(storageDtos);
            var tagEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => storageEntities.Contains(e))
                .SelectMany(e => e.StorageTags)
                .Select(e => e.Tag)
                .ToList();
            var tagDtos = Mapper.Map<IEnumerable<TagDto>>(tagEntities);
            return tagDtos;
        }

        public int UpdateStorageState(StorageDto storageDto)
        {
            var result = 0;
            try
            {
                StorageEntity entity = Mapper.Map<StorageEntity>(storageDto);
                UnitOfWork.Begin();
                UnitOfWork.GetRepository<StorageEntity>().Update(entity);
                if (storageDto.State == StorageConst.State.EmptyContainer)
                {
                    UnitOfWork
                        .GetRepository<StorageContainerEntity>()
                        .Delete(e => e.StorageId == entity.Id);
                }
                UnitOfWork.Commit();
                result = 1;
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
            return result;
        }

        public int ContainerAdd(string keyValue, IEnumerable<ContainerDto> containerDtos)
        {
            var containerIds = containerDtos.Select(e => e.Id);
            return ContainerAdd(keyValue, containerIds);
        }

        public int ContainerAdd(string keyValue, IEnumerable<string> containerIds)
        {
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var containerId in containerIds)
                {
                    var entity = new StorageContainerEntity()
                    {
                        StorageId = keyValue,
                        ContainerId = containerId
                    };
                    var isExist = UnitOfWork
                        .GetRepository<StorageContainerEntity>()
                        .Any(e =>
                            e.StorageId == entity.StorageId && e.ContainerId == entity.ContainerId
                        );
                    if (!isExist)
                    {
                        UnitOfWork.GetRepository<StorageContainerEntity>().Add(entity);
                        UnitOfWork
                            .GetRepository<StorageContainerHistoryEntity>()
                            .Add(
                                new StorageContainerHistoryEntity()
                                {
                                    StorageId = entity.StorageId,
                                    ContainerId = entity.ContainerId,
                                    State = StorageContainerHistoryConst.State.Add
                                }
                            );
                        result++;
                    }
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
            return result;
        }

        public int ContainerDelete(string keyValue, IEnumerable<string> containerIds)
        {
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var containerId in containerIds)
                {
                    var entity = new StorageContainerEntity()
                    {
                        StorageId = keyValue,
                        ContainerId = containerId
                    };
                    var isExist = UnitOfWork
                        .GetRepository<StorageContainerEntity>()
                        .Any(e =>
                            e.StorageId == entity.StorageId && e.ContainerId == entity.ContainerId
                        );
                    if (isExist)
                    {
                        UnitOfWork
                            .GetRepository<StorageContainerEntity>()
                            .Delete(e =>
                                e.StorageId == entity.StorageId
                                && e.ContainerId == entity.ContainerId
                            );
                        UnitOfWork
                            .GetRepository<StorageContainerHistoryEntity>()
                            .Add(
                                new StorageContainerHistoryEntity()
                                {
                                    StorageId = entity.StorageId,
                                    ContainerId = entity.ContainerId,
                                    State = StorageContainerHistoryConst.State.Delete
                                }
                            );
                        result++;
                    }
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
            return result;
        }

        public int ContainerDelete(string keyValue, IEnumerable<ContainerDto> containerDtos)
        {
            var containerIds = containerDtos.Select(e => e.Id);
            return ContainerDelete(keyValue, containerIds);
        }

        public int TagAdd(string keyValue, IEnumerable<TagDto> tagDtos)
        {
            var tagIds = tagDtos.Select(e => e.Id);
            return TagAdd(keyValue, tagIds);
        }

        public int TagAdd(string keyValue, IEnumerable<string> tagIds)
        {
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var tagId in tagIds)
                {
                    var entity = new StorageTagEntity() { TagId = tagId, StorageId = keyValue };
                    var isExist = UnitOfWork
                        .GetRepository<StorageTagEntity>()
                        .Any(e => e.TagId == entity.TagId && e.StorageId == entity.StorageId);
                    if (!isExist)
                    {
                        UnitOfWork.GetRepository<StorageTagEntity>().Add(entity);
                        result++;
                    }
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
            return result;
        }

        public int TagDelete(string keyValue, IEnumerable<string> tagIds)
        {
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var tagId in tagIds)
                {
                    var entity = new StorageTagEntity() { TagId = tagId, StorageId = keyValue };
                    var isExist = UnitOfWork
                        .GetRepository<StorageTagEntity>()
                        .Any(e => e.TagId == entity.TagId && e.StorageId == entity.StorageId);
                    if (isExist)
                    {
                        UnitOfWork
                            .GetRepository<StorageTagEntity>()
                            .Delete(e =>
                                e.TagId == entity.TagId && e.StorageId == entity.StorageId
                            );
                        result++;
                    }
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
            return result;
        }

        public int TagDelete(string keyValue, IEnumerable<TagDto> tagDtos)
        {
            var tagIds = tagDtos.Select(e => e.Id);
            return TagDelete(keyValue, tagIds);
        }
    }
}
