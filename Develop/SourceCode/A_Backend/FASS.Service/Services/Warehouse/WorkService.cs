using AutoMapper;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.NETCore.Extensions;
using Common.Service.Service;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Flow;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Data;
using FASS.Data.Entities.Flow;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Entities.FlowExtend;
using FASS.Service.Extends.Flow;
using FASS.Service.Services.Warehouse.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FASS.Service.Services.Warehouse
{
    public class WorkService : AuditService<FrameContext, WorkEntity, WorkDto>, IWorkService
    {
        public WorkService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, WorkEntity> repository,
            IMapper mapper,
            IValidator<WorkDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

        public async Task<int> DeleteM3Async(string? type = null)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddMonths(-3));
            }
            else
            {
                return await Repository.ExecuteDeleteAsync(e => e.Type == type && e.CreateAt < DateTime.Now.AddMonths(-3));
            }
        }

        public async Task<int> DeleteM1Async(string? type = null)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddMonths(-1));
            }
            else
            {
                return await Repository.ExecuteDeleteAsync(e => e.Type == type && e.CreateAt < DateTime.Now.AddMonths(-1));
            }
        }

        public async Task<int> DeleteW1Async(string? type = null)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddDays(-7));
            }
            else
            {
                return await Repository.ExecuteDeleteAsync(e => e.Type == type && e.CreateAt < DateTime.Now.AddDays(-7));
            }
        }

        public async Task<int> DeleteD1Async(string? type = null)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddDays(-1));
            }
            else
            {
                return await Repository.ExecuteDeleteAsync(e => e.Type == type && e.CreateAt < DateTime.Now.AddDays(-1));
            }
        }

        public async Task<int> DeleteAllAsync(string? type = null)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return await Repository.ExecuteDeleteAsync(e => true);
            }
            else
            {
                return await Repository.ExecuteDeleteAsync(e => e.Type == type && true);
            }
        }

        public async Task<int> DeleteDayAsync(string? type = null, int day = 90)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddDays(-day));
            }
            else
            {
                return await Repository.ExecuteDeleteAsync(e => e.Type == type && e.CreateAt < DateTime.Now.AddDays(-day));
            }
        }

        public async Task<int> AddWorkAsync(string areaId, string callModeName, TaskRecordDto taskRecordDto, ContainerDto containerDto, string material, string workType = "")
        {
            var result = 0;
            var taskRecordEntity = Mapper.Map<TaskRecordEntity>(taskRecordDto);
            try
            {
                await UnitOfWork.BeginAsync();
                var car = string.IsNullOrEmpty(taskRecordDto.CarId) ? default : await UnitOfWork.GetRepository<CarEntity>().FirstOrDefaultAsync(e => e.IsEnable && e.Id == taskRecordDto.CarId);
                //锁定起点和终点工位
                var storageEntitys = await UnitOfWork.GetRepository<StorageEntity>().ToListAsync(e => (e.Id == taskRecordDto.SrcStorageId || e.Id == taskRecordDto.DestStorageId) && e.IsLock == false);
                if (storageEntitys.Count > 0) 
                {
                    // await UnitOfWork.GetRepository<StorageEntity>().UpdateAsync(storageEntitys);
                    var ids = storageEntitys.Select(e => e.Id).ToList();
                    await UnitOfWork.GetRepository<StorageEntity>().ExecuteUpdateAsync(e => ids.Contains(e.Id), s => s.SetProperty(b => b.IsLock, true));
                }
                taskRecordEntity.CallMode = workType;
                await UnitOfWork.GetRepository<TaskRecordEntity>().AddAsync(taskRecordEntity);
                var extend = new TaskInstanceExtend()
                {
                    Material = material,
                    ContainerSize = new Models.FlowExtend.ContainerSize()
                    {
                        Length = containerDto.Length,
                        Width = containerDto.Width,
                        Height = containerDto.Height
                    },
                    Priority = taskRecordDto.Priority
                };
                var taskInstanceAdd = new TaskInstanceDto()
                {
                    Id = taskRecordDto.Id,
                    TaskTemplateId = taskRecordDto.TaskTemplateId,
                    CarId = car?.Id,
                    CarTypeId = string.IsNullOrEmpty(taskRecordDto.CarTypeId) ? null : taskRecordDto.CarTypeId,
                    Priority = taskRecordDto.Priority,
                    Code = string.IsNullOrEmpty(taskRecordDto.Code) ? $"{taskRecordDto.SrcNodeCode} => {taskRecordDto.DestNodeCode}" : taskRecordDto.Code,
                    Name = $"车辆 [{car?.Code}] 从起点站点 [{taskRecordDto.SrcNodeCode}] 到终点站点 [{taskRecordDto.DestNodeCode}]",
                    Type = TaskInstanceConst.Type.Normal,
                    State = TaskInstanceConst.State.Created,
                    //Remark = taskRecordDto.Remark,
                    Nodes = [taskRecordDto.SrcNodeId, taskRecordDto.DestNodeId],
                    Extend = extend.ToJson()
                };
                var taskInstanceEntity = Mapper.Map<TaskInstanceEntity>(taskInstanceAdd);
                await UnitOfWork.GetRepository<TaskInstanceEntity>().AddAsync(taskInstanceEntity);
                var workAdd = new WorkDto()
                {
                    ContainerId = containerDto.Id,
                    Code = taskRecordDto.Code ??"",
                    Name = callModeName,
                    AreaId = areaId,
                    Type = WorkConst.Type.Normal,
                    TaskId = taskInstanceAdd.Id,
                    TaskCode = taskRecordDto.Code,
                    State = WorkConst.State.Created,
                    Extend = workType
                };
                var worlEntity = Mapper.Map<WorkEntity>(workAdd);
                await UnitOfWork.GetRepository<WorkEntity>().AddAsync(worlEntity);
                await UnitOfWork.CommitAsync();
                result = 1;
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> UpdateWorkStateAsync(string keyValue, string state)
        {
            var result = await Repository.ExecuteUpdateAsync(e1 => e1.Id == keyValue, e2 => e2.SetProperty(e => e.State, state));
            return result;
        }
    }
}