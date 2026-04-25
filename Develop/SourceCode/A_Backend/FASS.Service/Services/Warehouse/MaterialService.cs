using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
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
    public class MaterialService : AuditService<FrameContext, MaterialEntity, MaterialDto>, IMaterialService
    {
        public MaterialService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, MaterialEntity> repository,
            IMapper mapper,
            IValidator<MaterialDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

        public async Task<IPage<MaterialDto>> SelectGetPageAsync(Page page)
        {
            var result = await Repository.Set()
                .ProjectTo<MaterialDto>(Mapper.ConfigurationProvider)
                .Where(e => e.IsEnable)
                .ToPageAsync(page);
            return result;
        }
        public async Task<IPage<MaterialDto>> SelectGetPageIsNoLockAsync(Page page)
        {
            var result = await Repository.Set()
                .AsNoTracking()
                .Where(e => e.IsEnable && !e.IsLock)
                .ProjectTo<MaterialDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return result;
        }
        public async Task<IPage<StorageDto>> StorageGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository.Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.MaterialStorages).Select(e => e.Storage)
                .ProjectTo<StorageDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }

        public async Task<int> StorageAddAsync(string keyValue, IEnumerable<string> storageIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                foreach (var storageId in storageIds)
                {
                    var entity = new MaterialStorageEntity() { MaterialId = keyValue, StorageId = storageId };
                    var isExist = await UnitOfWork.GetRepository<MaterialStorageEntity>().AnyAsync(e => e.MaterialId == entity.MaterialId && e.StorageId == entity.StorageId);
                    if (!isExist)
                    {
                        await UnitOfWork.GetRepository<MaterialStorageEntity>().AddAsync(entity);
                        await UnitOfWork.GetRepository<MaterialStorageHistoryEntity>().AddAsync(new MaterialStorageHistoryEntity()
                        {
                            MaterialId = entity.MaterialId,
                            StorageId = entity.StorageId,
                            State = MaterialStorageHistoryConst.State.Add
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
        }

        public async Task<int> StorageAddAsync(string keyValue, IEnumerable<StorageDto> storageDtos)
        {
            var storageIds = storageDtos.Select(e => e.Id);
            return await StorageAddAsync(keyValue, storageIds);
        }

        public async Task<int> StorageDeleteAsync(string keyValue, IEnumerable<string> storageIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                foreach (var storageId in storageIds)
                {
                    var entity = new MaterialStorageEntity() { MaterialId = keyValue, StorageId = storageId };
                    var isExist = await UnitOfWork.GetRepository<MaterialStorageEntity>().AnyAsync(e => e.MaterialId == entity.MaterialId && e.StorageId == entity.StorageId);
                    if (isExist)
                    {
                        await UnitOfWork.GetRepository<MaterialStorageEntity>().DeleteAsync(e => e.MaterialId == entity.MaterialId && e.StorageId == entity.StorageId);
                        await UnitOfWork.GetRepository<MaterialStorageHistoryEntity>().AddAsync(new MaterialStorageHistoryEntity()
                        {
                            MaterialId = entity.MaterialId,
                            StorageId = entity.StorageId,
                            State = MaterialStorageHistoryConst.State.Delete
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
        }

        public async Task<int> StorageDeleteAsync(string keyValue, IEnumerable<StorageDto> storageDtos)
        {
            var storageIds = storageDtos.Select(e => e.Id);
            return await StorageDeleteAsync(keyValue, storageIds);
        }

        public IEnumerable<StorageDto> GetStorages(MaterialDto materialDto)
        {
            var materialEntity = Mapper.Map<MaterialEntity>(materialDto);
            var storageEntities = Repository.Set()
                .AsNoTracking()
                .Where(e => e == materialEntity)
                .SelectMany(e => e.MaterialStorages).Select(e => e.Storage)
                .ToList();
            var storageDtos = Mapper.Map<IEnumerable<StorageDto>>(storageEntities);
            return storageDtos;
        }

        public IEnumerable<StorageDto> GetStorages(IEnumerable<MaterialDto> materialDtos)
        {
            var materialEntities = Mapper.Map<IEnumerable<MaterialEntity>>(materialDtos);
            var storageEntities = Repository.Set()
                .AsNoTracking()
                .Where(e => materialEntities.Contains(e))
                .SelectMany(e => e.MaterialStorages).Select(e => e.Storage)
                .ToList();
            var storageDtos = Mapper.Map<IEnumerable<StorageDto>>(storageEntities);
            return storageDtos;
        }

        public async Task<IPage<ContainerDto>> ContainerGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository.Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.ContainerMaterials).Select(e => e.Container)
                .ProjectTo<ContainerDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }

        #region 老版物料添加容器
        /*  public async Task<int> ContainerAddAsync(string keyValue, IEnumerable<string> containerIds)
          {
              var result = 0;
              try
              {
                  await UnitOfWork.BeginAsync();
                  foreach (var containerId in containerIds)
                  {
                      var entity = new ContainerMaterialEntity() { ContainerId = containerId, MaterialId = keyValue };
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

            try
            {
                await UnitOfWork.BeginAsync();

                var containerRepo = UnitOfWork.GetRepository<ContainerEntity>();
                var containerMaterialRepo = UnitOfWork.GetRepository<ContainerMaterialEntity>();
                var containerMaterialHistoryRepo = UnitOfWork.GetRepository<ContainerMaterialHistoryEntity>();
                var materialEntityRepo = UnitOfWork.GetRepository<MaterialEntity>();
                var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                var storageRepo = UnitOfWork.GetRepository<StorageEntity>();

                // Check if the material already has a container
                var existingEntity = await containerMaterialRepo.Set()
                    .FirstOrDefaultAsync(e => e.MaterialId == keyValue);
                if (existingEntity != null)
                {
                    // Material already bound to a container
                    return 0;
                }
                var existingContanierMaterialEntity = await containerMaterialRepo.Set()
                   .FirstOrDefaultAsync(e => containerIds.Contains(e.ContainerId));
                if (existingContanierMaterialEntity!=null)
                {
                    return 0;
                }
                // Fetch the first matching container
                var container = await containerRepo.Set()
                    .FirstOrDefaultAsync(c => containerIds.Contains(c.Id));
                if (container == null)
                {
                    // No valid container found
                    return 0;
                }
           
                // Fetch the material entity
                var materialEntity = await materialEntityRepo.Set()
                    .FirstOrDefaultAsync(e => e.Id == keyValue);
                if (materialEntity == null)
                {
                    // Material entity not found
                    throw new InvalidOperationException("Material entity not found.");
                }

                // Create new container-material relationship
                var containerMaterial = new ContainerMaterialEntity
                {
                    ContainerId = container.Id,
                    MaterialId = keyValue
                };
                await containerMaterialRepo.AddAsync(containerMaterial);

                // Create container-material history record
                var containerMaterialHistory = new ContainerMaterialHistoryEntity
                {
                    ContainerId = container.Id,
                    MaterialId = keyValue,
                    State = ContainerMaterialHistoryConst.State.Add
                };
                await containerMaterialHistoryRepo.AddAsync(containerMaterialHistory);

                // Update material and container states
               //materialEntity.IsLock = true;
                materialEntity.State = MaterialConst.State.Bind;
                await materialEntityRepo.UpdateAsync(materialEntity);

                //container.IsLock = true;
                container.State = ContainerConst.State.FullMaterial;
                await containerRepo.UpdateAsync(container);

                // Update storage state if necessary
                var storageContainer = await storageContainerRepo.Set()
                    .FirstOrDefaultAsync(e => e.ContainerId == container.Id);
                if (storageContainer != null)
                {
                    var storageEntity = await storageRepo.Set()
                        .FirstOrDefaultAsync(e => e.Id == storageContainer.StorageId);
                    if (storageEntity != null)
                    {
                        UpdateStorageEntityState(storageEntity, container);
                        await storageRepo.UpdateAsync(storageEntity);
                    }
                }

                await UnitOfWork.CommitAsync();
                return 1;
            }
            catch (Exception)
            {
                await UnitOfWork.RollbackAsync();
                // Log the error
                //Logger.LogError(ex, "Error occurred while adding containers.");
                throw;
            }
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
        public async Task<int> ContainerAddAsync(string keyValue, IEnumerable<ContainerDto> containerDtos)
        {
            var containerIds = containerDtos.Select(e => e.Id);
            return await ContainerAddAsync(keyValue, containerIds);
        }
        public async Task<int> ContainerDeleteAsync(string keyValue, IEnumerable<string> containerIds)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                throw new ArgumentException("Key value cannot be null or empty.", nameof(keyValue));
            }

            if (containerIds == null || !containerIds.Any())
            {
                return 0;
            }

            var result = 0;

            try
            {
                await UnitOfWork.BeginAsync();

                var containerRepo = UnitOfWork.GetRepository<ContainerEntity>();
                var containerMaterialRepo = UnitOfWork.GetRepository<ContainerMaterialEntity>();
                var containerMaterialHistoryRepo = UnitOfWork.GetRepository<ContainerMaterialHistoryEntity>();
                var materialEntityRepo = UnitOfWork.GetRepository<MaterialEntity>();
                var storageContainerRepo = UnitOfWork.GetRepository<StorageContainerEntity>();
                var storageRepo = UnitOfWork.GetRepository<StorageEntity>();

                // 获取物料实体
                var materialEntity = await materialEntityRepo.Set().FirstOrDefaultAsync(e => e.Id == keyValue);
                if (materialEntity == null)
                {
                    return 0;
                }

                foreach (var containerId in containerIds)
                {
                    // 删除容器与物料关系
                    var isExist = await containerMaterialRepo.AnyAsync(e => e.ContainerId == containerId && e.MaterialId == keyValue);
                    if (isExist)
                    {
                        await containerMaterialRepo.DeleteAsync(e => e.ContainerId == containerId && e.MaterialId == keyValue);
                        await containerMaterialHistoryRepo.AddAsync(new ContainerMaterialHistoryEntity()
                        {
                            ContainerId = containerId,
                            MaterialId = keyValue,
                            State = ContainerMaterialHistoryConst.State.Delete
                        });

                        // 修改物料状态
                        materialEntity.State = MaterialConst.State.UnBind;
                       //materialEntity.IsLock = false;
                        await materialEntityRepo.UpdateAsync(materialEntity);

                        // 修改容器状态
                        var container = await containerRepo.FirstOrDefaultAsync(e => e.Id == containerId);
                        if (container != null)
                        {
                            container.State = ContainerConst.State.EmptyMaterial;
                            //container.IsLock = false;
                            await containerRepo.UpdateAsync(container);

                            // 查询容器与库位关系
                            var storageContainer = await storageContainerRepo.FirstOrDefaultAsync(e => e.ContainerId == container.Id);
                            if (storageContainer != null)
                            {
                                // 获取库位信息
                                var storage = await storageRepo.FirstOrDefaultAsync(e => e.Id == storageContainer.StorageId);
                                if (storage != null)
                                {
                                    UpdateStorageEntityState(storage, container);
                                    await storageRepo.UpdateAsync(storage);
                                }
                            }
                        }
                    }
                }

                await UnitOfWork.CommitAsync();
                result = 1;
            }
            catch (Exception)
            {
                await UnitOfWork.RollbackAsync();
                // Log the error
                //  Logger.LogError(ex, "Error occurred while deleting containers.");
                throw;
            }

            return result;
        }

        public async Task<int> ContainerDeleteAsync(string keyValue, IEnumerable<ContainerDto> containerDtos)
        {
            var containerIds = containerDtos.Select(e => e.Id);
            return await ContainerDeleteAsync(keyValue, containerIds);
        }

        public IEnumerable<ContainerDto> GetContainers(MaterialDto materialDto)
        {
            var materialEntity = Mapper.Map<MaterialEntity>(materialDto);
            var containerEntities = Repository.Set()
                .AsNoTracking()
                .Where(e => e == materialEntity)
                .SelectMany(e => e.ContainerMaterials).Select(e => e.Container)
                .ToList();
            var containerDtos = Mapper.Map<IEnumerable<ContainerDto>>(containerEntities);
            return containerDtos;
        }

        public IEnumerable<ContainerDto> GetContainers(IEnumerable<MaterialDto> materialDtos)
        {
            var materialEntities = Mapper.Map<IEnumerable<MaterialEntity>>(materialDtos);
            var containerEntities = Repository.Set()
                .AsNoTracking()
                .Where(e => materialEntities.Contains(e))
                .SelectMany(e => e.ContainerMaterials).Select(e => e.Container)
                .ToList();
            var containerDtos = Mapper.Map<IEnumerable<ContainerDto>>(containerEntities);
            return containerDtos;
        }

        public async Task<IEnumerable<ContainerDto>> GetContainersAsync(MaterialDto materialDto)
        {
            var materialEntity = Mapper.Map<MaterialEntity>(materialDto);
            var containerEntities = await Repository.Set()
                .AsNoTracking()
                .Where(e => e == materialEntity)
                .SelectMany(e => e.ContainerMaterials).Select(e => e.Container)
                .ToListAsync();
            var containerDtos = Mapper.Map<IEnumerable<ContainerDto>>(containerEntities);
            return containerDtos;
        }

        public async Task<IEnumerable<ContainerDto>> GetContainersAsync(IEnumerable<MaterialDto> materialDtos)
        {
            var materialEntities = Mapper.Map<IEnumerable<MaterialEntity>>(materialDtos);
            var containerEntities = await Repository.Set()
                .AsNoTracking()
                .Where(e => materialEntities.Contains(e))
                .SelectMany(e => e.ContainerMaterials).Select(e => e.Container)
                .ToListAsync();
            var containerDtos = Mapper.Map<IEnumerable<ContainerDto>>(containerEntities);
            return containerDtos;
        }

        public async Task<IEnumerable<StorageDto>> GetStoragesByMaterialTypeAsync(string area, string materialType, bool isLock)
        {
            var storageEntities = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.Type == materialType && e.State == MaterialConst.State.Bind)
                .SelectMany(e => e.ContainerMaterials)
                .Select(e => e.Container)
                .SelectMany(e => e.StorageContainers)
                .Select(e => e.Storage)
                .Where(e => e.Area.Code == area && e.IsLock == isLock && e.IsEnable)
                .ToListAsync();
            var storageDtos = Mapper.Map<IEnumerable<StorageDto>>(storageEntities);
            return storageDtos;
        }

    }
}
