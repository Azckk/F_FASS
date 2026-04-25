using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.NETCore;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Service;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace FASS.Service.Services.Warehouse
{
    public class ContainerService
        : AuditService<FrameContext, ContainerEntity, ContainerDto>,
            IContainerService
    {
        public ContainerService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, ContainerEntity> repository,
            IMapper mapper,
            IValidator<ContainerDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }

        public override async Task<int> AddOrUpdateAsync(string? keyValue, ContainerDto containerDto)
        {
            // 初始化结果变量
            var result = 0;
            try
            {
                var containerEntity = Mapper.Map<ContainerEntity>(containerDto);
                // 开始事务
                await UnitOfWork.BeginAsync();

                // 获取相关仓储库
                var containerMaterialRepository = UnitOfWork.GetRepository<ContainerMaterialEntity>();
                var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                var storageRepo = UnitOfWork.GetRepository<StorageEntity>();
                var containerRepo = UnitOfWork.GetRepository<ContainerEntity>();

                // 如果 keyValue 为空或 null，则添加新容器
                if (string.IsNullOrEmpty(keyValue))
                {
                    await containerRepo.AddAsync(containerEntity);
                    result = 1;
                }
                else
                {
                    // 根据 keyValue 查找容器
                    var container = await Set().AsNoTracking().FirstOrDefaultAsync(e => e.Id == keyValue);
                    //var container = await containerRepo.FirstOrDefaultAsync(e => e.Id == keyValue);

                    if (container == null)
                    {
                        // 如果容器不存在，则添加新容器
                        await containerRepo.AddAsync(containerEntity);
                        result = 1;
                    }
                    else
                    {
                        // 获取与容器相关的物料关系
                        var existingMaterial = await containerMaterialRepository
                            .Set().FirstOrDefaultAsync(e => e.ContainerId == container.Id);

                        // 如果存在物料关系并且容器状态从满变空，则返回 0
                        if (existingMaterial != null && container.State == ContainerConst.State.FullMaterial
                            && containerDto.State == ContainerConst.State.EmptyMaterial)
                        {
                            result = 0;
                        }
                        else
                        {
                            // 更新容器信息
                            containerEntity.Id = container.Id;
                            await containerRepo.UpdateAsync(containerEntity);

                            // 更新库位状态
                            await UpdateStorageState(storageContainerRepo, storageRepo, keyValue, container.State);

                            result = 1;
                        }
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

        private async Task UpdateStorageState(IRepository<Context, StorageContainerEntity> storageContainerRepo,
            IRepository<Context, StorageEntity> storageRepo, string keyValue, string containerState)
        {
            // 获取容器与库位的关系
            var storageContainer = await storageContainerRepo.FirstOrDefaultAsync(e => e.ContainerId == keyValue);

            if (storageContainer != null)
            {
                // 获取库位信息并更新其状态
                var storage = await storageRepo.FirstOrDefaultAsync(e => e.Id == storageContainer.StorageId);

                if (storage != null)
                {
                    storage.State = containerState == ContainerConst.State.EmptyMaterial
                        ? StorageConst.State.EmptyContainer
                        : StorageConst.State.FullContainer;
                    await storageRepo.UpdateAsync(storage);
                }
            }
        }
        public async Task<IPage<ContainerDto>> SelectGetPageAsync(Page page)
        {
            var result = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.IsEnable)
                .ProjectTo<ContainerDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return result;
        }

        public async Task<IPage<MaterialDto>> MaterialGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.ContainerMaterials)
                .Select(e => e.Material)
                .ProjectTo<MaterialDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }

        public async Task<IEnumerable<ContainerMaterialDto>> GetContainerMaterialListAsync(
            string keyValue
        )
        {
            var dto = await Repository
                .Set()
                .AsNoTracking()
                .Where(e =>
                    string.IsNullOrEmpty(keyValue) ? e.IsEnable : (e.Id == keyValue && e.IsEnable)
                )
                .SelectMany(e => e.ContainerMaterials)
                .ProjectTo<ContainerMaterialDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return dto;
        }
        #region 老版本添加物料
        /*public async Task<int> MaterialAddAsync(string keyValue, IEnumerable<string> materialIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                foreach (var materialId in materialIds)
                {
                    var entity = new ContainerMaterialEntity() { ContainerId = keyValue, MaterialId = materialId };
                    var isExist = await UnitOfWork.GetRepository<ContainerMaterialEntity>().AnyAsync(e => e.ContainerId == entity.ContainerId && e.MaterialId == entity.MaterialId);
                    if (!isExist)
                    {
                        await UnitOfWork.GetRepository<ContainerMaterialEntity>().AddAsync(entity);
                        await UnitOfWork.GetRepository<ContainerMaterialHistoryEntity>().AddAsync(new ContainerMaterialHistoryEntity()
                        {
                            ContainerId = entity.ContainerId,
                            MaterialId = entity.MaterialId,
                            State = ContainerMaterialHistoryConst.State.Add
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

        public async Task<int> MaterialAddAsync(string keyValue, IEnumerable<string> materialIds)
        {
            var result = 0;
            var maxRetryCount = 3;
            var retryCount = 0;

            while (retryCount < maxRetryCount)
            {
                try
                {
                    await UnitOfWork.BeginAsync();

                    var containerRepository = UnitOfWork.GetRepository<ContainerEntity>();
                    var containerMaterialRepository = UnitOfWork.GetRepository<ContainerMaterialEntity>();
                    var containerMaterialHistoryRepository = UnitOfWork.GetRepository<ContainerMaterialHistoryEntity>();
                    var materialRepository = UnitOfWork.GetRepository<MaterialEntity>();
                    var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                    var storageRepo = UnitOfWork.GetRepository<StorageEntity>();

                    var containerEntity = await containerRepository.FirstOrDefaultAsync(e => e.Id == keyValue);
                    if (containerEntity == null)
                    {
                        throw new Exception($"Container with ID {keyValue} not found.");
                    }

                    var storageContainerEntity = await storageContainerRepo.Set().FirstOrDefaultAsync(e => e.ContainerId == keyValue);
                    var existingMaterialIds = await containerMaterialRepository.Set().Where(e => e.ContainerId == keyValue).Select(e => e.MaterialId).ToListAsync();
                    //判断如果物料已经绑定过容器，则需要提示，添加的物料已绑定过容器
                    if (existingMaterialIds.Any())
                    {
                        return 0;
                    }
                    var materialEntities = await materialRepository.Set().Where(e => e.IsEnable).ToListAsync();

                    var newContainerMaterials = new List<ContainerMaterialEntity>();
                    var newContainerMaterialHistories = new List<ContainerMaterialHistoryEntity>();

                    foreach (var materialId in materialIds)
                    {
                        if (!existingMaterialIds.Contains(materialId))
                        {
                            var entity = new ContainerMaterialEntity
                            {
                                ContainerId = keyValue,
                                MaterialId = materialId
                            };
                            newContainerMaterials.Add(entity);

                            var historyEntity = new ContainerMaterialHistoryEntity
                            {
                                ContainerId = entity.ContainerId,
                                MaterialId = entity.MaterialId,
                                State = ContainerMaterialHistoryConst.State.Add
                            };
                            newContainerMaterialHistories.Add(historyEntity);

                            var materialEntity = materialEntities.FirstOrDefault(e => e.Id == materialId);
                            if (materialEntity != null)
                            {
                                //materialEntity.IsLock = true;
                                materialEntity.State = MaterialConst.State.Bind;
                                await materialRepository.UpdateAsync(materialEntity);
                            }

                            result++;
                        }

                    }

                    if (newContainerMaterials.Any())
                    {
                        await containerMaterialRepository.AddAsync(newContainerMaterials);
                    }
                    if (newContainerMaterialHistories.Any())
                    {
                        await containerMaterialHistoryRepository.AddAsync(newContainerMaterialHistories);
                    }

                    if (result > 0)
                    {
                        //containerEntity.State = ContainerConst.State.FullMaterial;
                        //await containerRepository.UpdateAsync(containerEntity);
                        await containerRepository.ExecuteUpdateAsync(e => e.Id == containerEntity.Id, s => s.SetProperty(b => b.State, ContainerConst.State.FullMaterial));

                        if (storageContainerEntity != null)
                        {
                            var storageEntity = await storageRepo.FirstOrDefaultAsync(e => e.Id == storageContainerEntity.StorageId);
                            if (storageEntity != null)
                            {
                                //storageEntity.State = StorageConst.State.FullContainer;
                                //await storageRepo.UpdateAsync(storageEntity);
                                await storageRepo.ExecuteUpdateAsync(e => e.Id == storageEntity.Id, s => s.SetProperty(b => b.State, StorageConst.State.FullContainer));
                            }
                        }
                    }

                    await UnitOfWork.CommitAsync();
                    break; // 如果成功，则跳出循环
                }
                catch (DbUpdateConcurrencyException)
                {
                    await UnitOfWork.RollbackAsync();
                    retryCount++;
                    if (retryCount >= maxRetryCount)
                    {
                        throw new Exception("A concurrency error occurred. Please try again.");
                    }
                }
                catch
                {
                    await UnitOfWork.RollbackAsync();
                    throw;
                }
            }

            return result;
        }

        public async Task<int> MaterialAddAsync(
            string keyValue,
            IEnumerable<MaterialDto> materialDtos
        )
        {
            var materialIds = materialDtos.Select(e => e.Id);
            return await MaterialAddAsync(keyValue, materialIds);
        }

        #region 老版本删物料
        /*public async Task<int> MaterialDeleteAsync(string keyValue, IEnumerable<string> materialIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                foreach (var materialId in materialIds)
                {
                    var entity = new ContainerMaterialEntity() { ContainerId = keyValue, MaterialId = materialId };
                    var isExist = await UnitOfWork.GetRepository<ContainerMaterialEntity>().AnyAsync(e => e.ContainerId == entity.ContainerId && e.MaterialId == entity.MaterialId);
                    if (isExist)
                    {
                        await UnitOfWork.GetRepository<ContainerMaterialEntity>().DeleteAsync(e => e.ContainerId == entity.ContainerId && e.MaterialId == entity.MaterialId);
                        await UnitOfWork.GetRepository<ContainerMaterialHistoryEntity>().AddAsync(new ContainerMaterialHistoryEntity()
                        {
                            ContainerId = entity.ContainerId,
                            MaterialId = entity.MaterialId,
                            State = ContainerMaterialHistoryConst.State.Delete
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

        public async Task<int> MaterialDeleteAsync(string keyValue, IEnumerable<string> materialIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();

                var materialRepository = UnitOfWork.GetRepository<MaterialEntity>();
                var containerRepository = UnitOfWork.GetRepository<ContainerEntity>();
                var containerMaterialRepository = UnitOfWork.GetRepository<ContainerMaterialEntity>();
                var containerMaterialHistoryRepository = UnitOfWork.GetRepository<ContainerMaterialHistoryEntity>();
                var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                var storageRepo = UnitOfWork.GetRepository<StorageEntity>();

                var containerEntity = await containerRepository.FirstOrDefaultAsync(e => e.Id == keyValue);
                if (containerEntity == null)
                {
                    throw new Exception($"Container with ID {keyValue} not found.");
                }

                var containerMaterials = await containerMaterialRepository
                    .Set()
                    .Where(e => e.ContainerId == keyValue)
                    .ToListAsync();
                var existingMaterialIds = containerMaterials
                    .Select(e => e.MaterialId)
                    .ToList();

                var materialEntities = await materialRepository
                    .Set()
                    .Where(e => e.IsEnable)
                    .ToListAsync();

                var materialsToDelete = new List<ContainerMaterialEntity>();
                var materialHistories = new List<ContainerMaterialHistoryEntity>();
                var materialsToUpdate = new List<MaterialEntity>();

                foreach (var materialId in materialIds)
                {
                    if (existingMaterialIds.Contains(materialId))
                    {
                        var entity = containerMaterials.FirstOrDefault(e => e.MaterialId == materialId);
                        if (entity != null)
                        {
                            materialsToDelete.Add(entity);

                            var historyEntity = new ContainerMaterialHistoryEntity
                            {
                                ContainerId = entity.ContainerId,
                                MaterialId = entity.MaterialId,
                                State = ContainerMaterialHistoryConst.State.Delete
                            };
                            materialHistories.Add(historyEntity);

                            var materialEntity = materialEntities.FirstOrDefault(e => e.Id == materialId);
                            if (materialEntity != null)
                            {
                                //materialEntity.IsLock = false;
                                materialEntity.State = MaterialConst.State.UnBind;
                                materialsToUpdate.Add(materialEntity);
                            }

                            result++;
                        }
                    }
                }

                if (materialsToDelete.Any())
                {
                    await containerMaterialRepository.DeleteAsync(materialsToDelete);
                }
                if (materialHistories.Any())
                {
                    await containerMaterialHistoryRepository.AddAsync(materialHistories);
                }
                if (materialsToUpdate.Any())
                {
                    await materialRepository.UpdateAsync(materialsToUpdate);
                }
                if (result == materialIds.Count())
                {
                    containerEntity.State = ContainerConst.State.EmptyMaterial;

                    var storageContainerDto = await storageContainerRepo
                        .Set()
                        .FirstOrDefaultAsync(e => e.ContainerId == containerEntity.Id);
                    if (storageContainerDto != null)
                    {
                        var storage = await storageRepo
                            .Set()
                            .FirstOrDefaultAsync(e => e.Id == storageContainerDto.StorageId);
                        if (storage != null)
                        {
                            storage.State = StorageConst.State.EmptyContainer;
                            await storageRepo.UpdateAsync(storage);
                        }
                    }

                    await containerRepository.UpdateAsync(containerEntity);
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
            var containerIds = materialDtos.Select(e => e.Id);
            return await MaterialDeleteAsync(keyValue, containerIds);
        }

        public async Task<int> MaterialDeleteAsync(string keyValue)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                var containerMaterialDtos = await UnitOfWork
                    .GetRepository<ContainerMaterialEntity>().ToListAsync(e => e.ContainerId == keyValue);
                var materialIds = containerMaterialDtos.Select(e => e.MaterialId);
                foreach (var materialId in materialIds)
                {
                    var entity = new ContainerMaterialEntity()
                    {
                        ContainerId = keyValue,
                        MaterialId = materialId
                    };
                    await UnitOfWork
                        .GetRepository<ContainerMaterialEntity>()
                        .DeleteAsync(e =>
                            e.ContainerId == entity.ContainerId && e.MaterialId == entity.MaterialId
                        );
                    await UnitOfWork
                        .GetRepository<ContainerMaterialHistoryEntity>()
                        .AddAsync(
                            new ContainerMaterialHistoryEntity()
                            {
                                ContainerId = entity.ContainerId,
                                MaterialId = entity.MaterialId,
                                State = ContainerMaterialHistoryConst.State.Delete
                            }
                        );
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

        public int MaterialDelete(string keyValue)
        {
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                var containerMaterialDtos = UnitOfWork
                    .GetRepository<ContainerMaterialEntity>()
                    .ToList(e => e.ContainerId == keyValue);
                var materialIds = containerMaterialDtos.Select(e => e.MaterialId);
                foreach (var materialId in materialIds)
                {
                    var entity = new ContainerMaterialEntity()
                    {
                        ContainerId = keyValue,
                        MaterialId = materialId
                    };
                    UnitOfWork
                        .GetRepository<ContainerMaterialEntity>()
                        .Delete(e =>
                            e.ContainerId == entity.ContainerId && e.MaterialId == entity.MaterialId
                        );
                    UnitOfWork
                        .GetRepository<ContainerMaterialHistoryEntity>()
                        .Add(
                            new ContainerMaterialHistoryEntity()
                            {
                                ContainerId = entity.ContainerId,
                                MaterialId = entity.MaterialId,
                                State = ContainerMaterialHistoryConst.State.Delete
                            }
                        );
                    result++;
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

        public async Task<IEnumerable<MaterialDto>> GetMaterialsAsync(ContainerDto containerDto)
        {
            var containerEntity = Mapper.Map<ContainerEntity>(containerDto);
            var materialEntities = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e == containerEntity)
                .SelectMany(e => e.ContainerMaterials)
                .Select(e => e.Material)
                .ToListAsync();
            var materialDtos = Mapper.Map<IEnumerable<MaterialDto>>(materialEntities);
            return materialDtos;
        }

        public async Task<IEnumerable<MaterialDto>> GetMaterialsAsync(
            IEnumerable<ContainerDto> containerDtos
        )
        {
            var containerEntities = Mapper.Map<IEnumerable<ContainerEntity>>(containerDtos);
            var materialEntities = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => containerEntities.Contains(e))
                .SelectMany(e => e.ContainerMaterials)
                .Select(e => e.Material)
                .ToListAsync();
            var materialDtos = Mapper.Map<IEnumerable<MaterialDto>>(materialEntities);
            return materialDtos;
        }

        public IEnumerable<MaterialDto> GetMaterials(ContainerDto containerDto)
        {
            var containerEntity = Mapper.Map<ContainerEntity>(containerDto);
            var materialEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => e == containerEntity)
                .SelectMany(e => e.ContainerMaterials)
                .Select(e => e.Material)
                .ToList();
            var materialDtos = Mapper.Map<IEnumerable<MaterialDto>>(materialEntities);
            return materialDtos;
        }

        public IEnumerable<MaterialDto> GetMaterials(string keyValue)
        {
            var materialEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.ContainerMaterials)
                .Select(e => e.Material)
                .ToList();
            var materialDtos = Mapper.Map<IEnumerable<MaterialDto>>(materialEntities);
            return materialDtos;
        }

        public IEnumerable<MaterialDto> GetMaterials(IEnumerable<ContainerDto> containerDtos)
        {
            var containerEntities = Mapper.Map<IEnumerable<ContainerEntity>>(containerDtos);
            var materialEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => containerEntities.Contains(e))
                .SelectMany(e => e.ContainerMaterials)
                .Select(e => e.Material)
                .ToList();
            var materialDtos = Mapper.Map<IEnumerable<MaterialDto>>(materialEntities);
            return materialDtos;
        }

        public async Task<IPage<StorageDto>> StorageGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Storage)
                .ProjectTo<StorageDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }
        #region 老版容器界面添加库位
        /*        public async Task<int> StorageAddAsync(string keyValue, IEnumerable<string> storageIds)
                {
                    var result = 0;
                    try
                    {
                        await UnitOfWork.BeginAsync();
                        foreach (var storageId in storageIds)
                        {
                            var entity = new StorageContainerEntity()
                            {
                                StorageId = storageId,
                                ContainerId = keyValue
                            };
                            var isExist = await UnitOfWork
                                .GetRepository<StorageContainerEntity>()
                                .AnyAsync(e =>
                                    e.StorageId == entity.StorageId && e.ContainerId == entity.ContainerId
                                );
                            if (!isExist)
                            {
                                await UnitOfWork.GetRepository<StorageContainerEntity>().AddAsync(entity);
                                await UnitOfWork
                                    .GetRepository<StorageContainerHistoryEntity>()
                                    .AddAsync(
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

        /// <summary>
        /// 容器界面添加库位操作
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="storageIds"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<int> StorageAddAsync(string keyValue, IEnumerable<string> storageIds)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                throw new ArgumentException("Key value cannot be null or empty.", nameof(keyValue));
            }

            if (storageIds == null || !storageIds.Any())
            {
                throw new ArgumentException("Container IDs cannot be null or empty.", nameof(storageIds));
            }

            var result = 0;

            try
            {
                await UnitOfWork.BeginAsync();
                if (storageIds.Count() > 1)
                {
                    return 0;
                }
                var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                var containerRepo = UnitOfWork.GetRepository<ContainerEntity>();
                var storageContainerHistoryRepo = UnitOfWork.GetRepository<StorageContainerHistoryEntity>();
                var storageRepo = UnitOfWork.GetRepository<StorageEntity>();

                var entitiesToAdd = new List<StorageContainerEntity>();
                var entitiesToUpdate = new List<StorageContainerEntity>();
                var historyEntitiesToAdd = new List<StorageContainerHistoryEntity>();
                var containersToUpdate = new List<ContainerEntity>();
                var storageToUpdate = new List<StorageEntity>();

                // 查询现有的 StorageContainerEntity
                var existingEntity = await storageContainerRepo.Set()
                    .FirstOrDefaultAsync(e => e.ContainerId == keyValue);

                // 查询参数 库位实体
                var storage = Guard.NotNull(await storageRepo.Set()
                    .Where(c => storageIds.Contains(c.Id)).FirstOrDefaultAsync());
                // 查询现有的 StorageContainerEntity
                var storageContainer = await storageContainerRepo.Set()
                    .FirstOrDefaultAsync(e => storageIds.Contains(e.StorageId));
                if (storageContainer != null)
                {
                    return 0;
                }
                //获取当前容器实体
                var containerEntity = Guard.NotNull(await containerRepo.FirstOrDefaultAsync(e => e.Id == keyValue));


                //如果当前容器没有绑定库位
                if (existingEntity == null)
                {
                    //绑定库位
                    if (containerEntity != null)
                    {
                        containerEntity.IsLock = true;
                        containersToUpdate.Add(containerEntity);
                        //await containerRepo.UpdateAsync(containerEntity);
                    }

                    //创建容器和库位绑定关系实体
                    var entity = new StorageContainerEntity
                    {
                        StorageId = storage.Id,
                        ContainerId = keyValue
                    };

                    // 根据容器状态修改库位状态
                    if (containerEntity != null)
                    {
                        UpdateStorageEntityState(storage, containerEntity);
                    }
                    //添加容器和库位关系
                    entitiesToAdd.Add(entity);
                    //await storageContainerRepo.AddAsync(entitiesToAdd);
                    //添加容器库位关系历史记录
                    historyEntitiesToAdd.Add(CreateHistoryEntity(entity, StorageContainerHistoryConst.State.Add));
                    //await storageContainerHistoryRepo.AddAsync(CreateHistoryEntity(entity, StorageContainerHistoryConst.State.Add));
                    //修改库位状态
                    // await storageRepo.UpdateAsync(storage);
                    storageToUpdate.Add(storage);
                    result = 1;
                }
                else
                {
                    //获取当前绑定的库位实体类
                    var currentStorage = Guard.NotNull(await storageRepo.FirstOrDefaultAsync(e => e.Id == existingEntity.StorageId));
                    //如果当前容器绑定的库位和传递过来的库位信息不一致 则更行绑定信息
                    if (storage != null && existingEntity.StorageId != storage.Id && storage.State == StorageConst.State.NoneContainer && !storage.IsLock)
                    {
                        //修改绑定的库位
                        existingEntity.StorageId = storage.Id;
                        //根据容器更新两个变化库位的状态
                        UpdateStorageEntityState(storage, containerEntity);
                        storageToUpdate.Add(storage);

                        entitiesToUpdate.Add(existingEntity);

                        //被移除的库位修改状态为空容器
                        currentStorage.State = StorageConst.State.NoneContainer;
                        storageToUpdate.Add(currentStorage);
                        //创建容器和库位绑定关系实体
                        var entity = new StorageContainerEntity
                        {
                            StorageId = storage.Id,
                            ContainerId = keyValue
                        };
                        // await storageContainerHistoryRepo.AddAsync(CreateHistoryEntity(entity, StorageContainerHistoryConst.State.Add));
                        historyEntitiesToAdd.Add(CreateHistoryEntity(entity, StorageContainerHistoryConst.State.Add));
                        //修改前一个库位和当前容器的记录信息
                        var entity2 = new StorageContainerEntity
                        {
                            StorageId = currentStorage.Id,
                            ContainerId = keyValue
                        };
                        historyEntitiesToAdd.Add(CreateHistoryEntity(entity, StorageContainerHistoryConst.State.Delete));
                        result = 1;
                    }
                    else { result = 0; }
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
                if (storageToUpdate.Any())
                {
                    await storageRepo.UpdateAsync(storageToUpdate);

                }


                await UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await UnitOfWork.RollbackAsync();
                // 记录异常日志
                // Logger.LogError(ex, "Error occurred while adding containers.");
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
        public async Task<int> StorageAddAsync(string keyValue, IEnumerable<StorageDto> storageDtos)
        {
            var storageIds = storageDtos.Select(e => e.Id);
            return await StorageAddAsync(keyValue, storageIds);
        }

        public async Task<int> StorageDeleteAsync(string keyValue, IEnumerable<string> storageIds)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                throw new ArgumentException("Key value cannot be null or empty.", nameof(keyValue));
            }

            if (storageIds == null || !storageIds.Any())
            {
                throw new ArgumentException("Storage IDs cannot be null or empty.", nameof(storageIds));
            }

            var result = 0;

            try
            {
                await UnitOfWork.BeginAsync();

                var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                var storageContainerHistoryRepo = UnitOfWork.GetRepository<StorageContainerHistoryEntity>();
                var containerRepo = UnitOfWork.GetRepository<ContainerEntity>();
                var storageRepo = UnitOfWork.GetRepository<StorageEntity>();

                foreach (var storageId in storageIds)
                {
                    var isExist = await storageContainerRepo.AnyAsync(e => e.StorageId == storageId && e.ContainerId == keyValue);
                    if (isExist)
                    {
                        await storageContainerRepo.DeleteAsync(e => e.StorageId == storageId && e.ContainerId == keyValue);
                        await storageContainerHistoryRepo.AddAsync(new StorageContainerHistoryEntity
                        {
                            StorageId = storageId,
                            ContainerId = keyValue,
                            State = StorageContainerHistoryConst.State.Delete
                        });

                        // 修改容器状态
                        var container = await containerRepo.FirstOrDefaultAsync(e => e.Id == keyValue);
                        if (container != null)
                        {
                            container.IsLock = false;
                            await containerRepo.UpdateAsync(container);
                        }

                        // 修改库位状态
                        var storage = await storageRepo.FirstOrDefaultAsync(e => e.Id == storageId);
                        if (storage != null)
                        {
                            storage.State = StorageConst.State.NoneContainer;
                            await storageRepo.UpdateAsync(storage);
                        }

                        result++;
                    }
                }

                await UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await UnitOfWork.RollbackAsync();
                // Log the error
                // Logger.LogError(ex, "Error occurred while deleting storage containers.");
                throw;
            }

            return result;
        }

        public async Task<int> StorageDeleteAsync(
            string keyValue,
            IEnumerable<StorageDto> storageDtos
        )
        {
            var storageIds = storageDtos.Select(e => e.Id);
            return await StorageDeleteAsync(keyValue, storageIds);
        }

        public IEnumerable<StorageDto> GetStorages(ContainerDto containerDto)
        {
            var containerEntity = Mapper.Map<ContainerEntity>(containerDto);
            var storageEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => e == containerEntity)
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Storage)
                .ToList();
            var storageDtos = Mapper.Map<IEnumerable<StorageDto>>(storageEntities);
            return storageDtos;
        }

        public IEnumerable<StorageDto> GetStorages(IEnumerable<ContainerDto> containerDtos)
        {
            var containerEntities = Mapper.Map<IEnumerable<ContainerEntity>>(containerDtos);
            var storageEntities = Repository
                .Set()
                .AsNoTracking()
                .Where(e => containerEntities.Contains(e))
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Storage)
                .ToList();
            var storageDtos = Mapper.Map<IEnumerable<StorageDto>>(storageEntities);
            return storageDtos;
        }

        public async Task<IEnumerable<StorageDto>> GetStoragesAsync(ContainerDto containerDto)
        {
            var containerEntity = Mapper.Map<ContainerEntity>(containerDto);
            var storageEntities = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e == containerEntity)
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Storage)
                .ToListAsync();
            var storageDtos = Mapper.Map<IEnumerable<StorageDto>>(storageEntities);
            return storageDtos;
        }

        public async Task<IEnumerable<StorageDto>> GetStoragesAsync(
            IEnumerable<ContainerDto> containerDtos
        )
        {
            var containerEntities = Mapper.Map<IEnumerable<ContainerEntity>>(containerDtos);
            var storageEntities = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => containerEntities.Contains(e))
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Storage)
                .ToListAsync();
            var storageDtos = Mapper.Map<IEnumerable<StorageDto>>(storageEntities);
            return storageDtos;
        }
    }
}
