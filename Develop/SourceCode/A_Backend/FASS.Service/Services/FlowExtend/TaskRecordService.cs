using AutoMapper;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using Common.Service.Service;
using FASS.Data.Consts.Flow;
using FASS.Data.Dtos.Flow;
using FASS.Data.Entities.Data;
using FASS.Data.Entities.Flow;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Entities.FlowExtend;
using FASS.Service.Extends.Flow;
using FASS.Service.Models.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.FlowExtend
{
    public class TaskRecordService : AuditService<FrameContext, TaskRecordEntity, TaskRecordDto>, ITaskRecordService
    {

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public TaskRecordService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, TaskRecordEntity> repository,
            IMapper mapper,
            IValidator<TaskRecordDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

        public override async Task<int> AddOrUpdateAsync(string? keyValue, TaskRecordDto taskRecordDto)
        {
            var result = 0;
            var taskRecordEntity = Mapper.Map<TaskRecordEntity>(taskRecordDto);
            try
            {
                await UnitOfWork.BeginAsync();
                var car = string.IsNullOrEmpty(taskRecordDto.CarId) ? default : await UnitOfWork.GetRepository<CarEntity>().FirstOrDefaultAsync(e => e.IsEnable && e.Id == taskRecordDto.CarId);
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var taskRecordEntity2 = await UnitOfWork.GetRepository<TaskRecordEntity>().FirstOrDefaultAsync(e => e.Id == keyValue);
                    if (taskRecordEntity2 != null)
                    {
                        taskRecordEntity2.CarId = taskRecordEntity.CarId;
                        taskRecordEntity2.CarTypeId = taskRecordEntity.CarTypeId;
                        taskRecordEntity2.Priority = taskRecordEntity.Priority;
                        taskRecordEntity2.SrcNodeId = taskRecordDto.SrcNodeId;
                        taskRecordEntity2.DestNodeId = taskRecordDto.DestNodeId;
                        taskRecordEntity2.IsLoop = taskRecordDto.IsLoop;
                        taskRecordEntity2.Condition = taskRecordDto.Condition;
                        taskRecordEntity2.Priority = taskRecordDto.Priority;
                        await UnitOfWork.GetRepository<TaskRecordEntity>().UpdateAsync(taskRecordEntity2);
                    }

                    var taskInstanceEntity = await UnitOfWork.GetRepository<TaskInstanceEntity>().FirstOrDefaultAsync(e => e.Id == keyValue);
                    if (taskInstanceEntity != null)
                    {
                        taskInstanceEntity.CarId = taskRecordDto.CarId;
                        taskInstanceEntity.CarTypeId = taskRecordDto.CarTypeId;
                        taskInstanceEntity.Priority = taskRecordDto.Priority;
                        taskInstanceEntity.Nodes = [taskRecordDto.SrcNodeId, taskRecordDto.DestNodeId];
                        taskInstanceEntity.Extend = new TaskInstanceExtend
                        {
                            Priority = taskRecordDto.Priority
                        }.ToJson();
                        await UnitOfWork.GetRepository<TaskInstanceEntity>().UpdateAsync(taskInstanceEntity);
                    }
                }
                else
                {
                    taskRecordEntity.Id = GuidHelper.GetGuidSequential().ToString();
                    taskRecordEntity.Name = $"车辆 [{car?.Code}] 从起点站点 [{taskRecordDto.SrcNodeCode}] 到终点站点 [{taskRecordDto.DestNodeCode}]";
                    if (string.IsNullOrWhiteSpace(taskRecordEntity.Code))
                    {
                        taskRecordEntity.Code = $"{taskRecordDto.SrcNodeCode} => {taskRecordDto.DestNodeCode}";
                    }
                    taskRecordEntity.IsDelete = false;
                    await UnitOfWork.GetRepository<TaskRecordEntity>().AddAsync(taskRecordEntity);
                    var taskInstanceAdd = new TaskInstanceDto()
                    {
                        Id = taskRecordEntity.Id,
                        TaskTemplateId = taskRecordDto.TaskTemplateId,
                        CarId = car?.Id,
                        CarTypeId = car?.CarTypeId,
                        Code = string.IsNullOrWhiteSpace(taskRecordDto.Code) ?
                          $"车辆 [{car?.Code}] 从起点站点 [{taskRecordDto.SrcNodeCode}] 到终点站点 [{taskRecordDto.DestNodeCode}]" : taskRecordDto.Code,
                        Name = $"车辆 [{car?.Code}] 从起点站点 [{taskRecordDto.SrcNodeCode}] 到终点站点 [{taskRecordDto.DestNodeCode}]",
                        Type = TaskInstanceConst.Type.Normal,
                        State = TaskInstanceConst.State.Created,
                        Priority = taskRecordDto.Priority,
                        Nodes = [taskRecordDto.SrcNodeId, taskRecordDto.DestNodeId],
                        Extend = new TaskInstanceExtend
                        {
                            Priority = taskRecordDto.Priority
                        }.ToJson()
                };
                    var taskInstanceEntity = Mapper.Map<TaskInstanceEntity>(taskInstanceAdd);
                    await UnitOfWork.GetRepository<TaskInstanceEntity>().AddAsync(taskInstanceEntity);
                }
                result = 1;
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public override async Task<int> DeleteAsync(IEnumerable<string> keyValues)
        {
            return await DeleteTaskRecordsAsync(keyValues);
        }

        public async Task<int> DeleteTaskRecordsAsync(IEnumerable<string> keyValues)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                foreach (var taskrecordId in keyValues)
                {
                    await UnitOfWork.GetRepository<TaskRecordEntity>().DeleteAsync(e => e.Id == taskrecordId);
                    await UnitOfWork.GetRepository<TaskInstanceEntity>().DeleteAsync(e => e.Id == taskrecordId);
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

        public int AddTaskRecord(TaskRecordDto taskRecordDto, string taskTemplateCode)
        {
            var result = 0;
            var taskRecordEntity = Mapper.Map<TaskRecordEntity>(taskRecordDto);
            try
            {
                UnitOfWork.Begin();
                var car = string.IsNullOrEmpty(taskRecordDto.CarId) ? default : UnitOfWork.GetRepository<CarEntity>().FirstOrDefault(e => e.IsEnable && e.Id == taskRecordDto.CarId);
                UnitOfWork.GetRepository<TaskRecordEntity>().Add(taskRecordEntity);
                var taskInstanceAdd = new TaskInstanceDto()
                {
                    Id = taskRecordDto.Id,
                    TaskTemplateId = taskTemplateCode,
                    CarId = car?.Id,
                    CarTypeId = taskRecordDto.CarTypeId,
                    Code = taskRecordDto.Code ?? "",
                    Name = $"车辆 [{car?.Code}] 从起点站点 [{taskRecordDto.SrcNodeCode}] 到终点站点 [{taskRecordDto.DestNodeCode}]",
                    Type = TaskInstanceConst.Type.Normal,
                    State = TaskInstanceConst.State.Created,
                    Nodes = [taskRecordDto.SrcNodeId, taskRecordDto.DestNodeId]
                };
                var taskInstanceEntity = Mapper.Map<TaskInstanceEntity>(taskInstanceAdd);
                UnitOfWork.GetRepository<TaskInstanceEntity>().Add(taskInstanceEntity);
                result = 1;
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
            return result;
        }

        public int UpdateTaskRecordState(string keyValue, string state)
        {
            var result = Repository.ExecuteUpdate(e1 => e1.Id == keyValue, e2 => e2.SetProperty(e => e.State, state));
            return result;
        }

        public async Task<int> UpdateTaskRecordStateAsync(string keyValue, string state)
        {
            var result = await Repository.ExecuteUpdateAsync(e1 => e1.Id == keyValue, e2 => e2.SetProperty(e => e.State, state));
            return result;
        }

        public int AddModelsWithOutTaskInstance(IEnumerable<TaskRecord> models)
        {
            var entities = Mapper.Map<IEnumerable<TaskRecordEntity>>(models);
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var entity in entities)
                {
                    var existEntity = UnitOfWork.GetRepository<TaskRecordEntity>().Set().Where(e => e.Id == entity.Id && e.TaskCreateAt == entity.TaskCreateAt).FirstOrDefault();
                    if (existEntity == null)
                    {
                        UnitOfWork.GetRepository<TaskRecordEntity>().Add(entity);
                        result++;
                    }
                    else
                    {
                        if (existEntity.State != entity.State)
                        {
                            existEntity.State = entity.State;
                            existEntity.CarId = entity.CarId;
                            existEntity.CarTypeId = entity.CarTypeId;
                            existEntity.StartTime = entity.StartTime;
                            existEntity.EndTime = entity.EndTime;
                            UnitOfWork.GetRepository<TaskRecordEntity>().Update(existEntity);
                            result++;
                        }
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

        public int AddModelWithOutTaskInstance(TaskRecord model)
        {
            var result = 0;
            var entity = Mapper.Map<TaskRecordEntity>(model);
            var isExist = Repository.Any(e => e.Id == entity.Id && e.TaskCreateAt == entity.TaskCreateAt);
            if (!isExist)
            {
                result = Repository.Add(entity);
            }
            return result;
        }

        public int UpdateModelsWithOutTaskInstance(IEnumerable<TaskRecord> models)
        {
            var entities = Mapper.Map<IEnumerable<TaskRecordEntity>>(models);
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var entity in entities)
                {
                    var existEntity = UnitOfWork.GetRepository<TaskRecordEntity>().Set().Where(e => e.Id == entity.Id && e.TaskCreateAt == entity.TaskCreateAt).FirstOrDefault();
                    if (existEntity != null)
                    {
                        existEntity.State = entity.State;
                        existEntity.CarId = entity.CarId;
                        existEntity.CarTypeId = entity.CarTypeId;
                        existEntity.StartTime = entity.StartTime;
                        existEntity.EndTime = entity.EndTime;
                        UnitOfWork.GetRepository<TaskRecordEntity>().Update(existEntity);
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

        public int UpdateModelWithOutTaskInstance(TaskRecord model)
        {
            var result = 0;
            var entity = Repository.Set().Where(e => e.Id == model.Id && e.TaskCreateAt == model.TaskCreateAt).FirstOrDefault();
            if (entity != null)
            {
                entity.State = model.State;
                entity.CarId = model.CarId;
                entity.CarTypeId = model.CarTypeId;
                entity.StartTime = model.StartTime;
                entity.EndTime = model.EndTime;

                result = Repository.Update(
                entity,
                e => e.State!,
                e => e.CarId!,
                e => e.CarTypeId!,
                e => e.StartTime!,
                e => e.EndTime!);
            }
            return result;
        }

        public async Task<int> AddTaskRecordAsync(TaskRecordDto taskRecordDto)
        {
            var result = 0;
            var taskRecordEntity = Mapper.Map<TaskRecordEntity>(taskRecordDto);
            try
            {
                await UnitOfWork.BeginAsync();
                var car = string.IsNullOrEmpty(taskRecordDto.CarId) ? default : await UnitOfWork.GetRepository<CarEntity>().FirstOrDefaultAsync(e => e.IsEnable && e.Id == taskRecordDto.CarId);
                await UnitOfWork.GetRepository<TaskRecordEntity>().AddAsync(taskRecordEntity);
                var taskInstanceAdd = new TaskInstanceDto()
                {
                    Id = taskRecordDto.Id,
                    TaskTemplateId = taskRecordDto.TaskTemplateId,
                    CarId = car?.Id,
                    CarTypeId = string.IsNullOrEmpty(taskRecordDto.CarTypeId) ? null : taskRecordDto.CarTypeId,
                    Code = string.IsNullOrEmpty(taskRecordDto.Code) ? $"{taskRecordDto.SrcNodeCode} => {taskRecordDto.DestNodeCode}" : taskRecordDto.Code,
                    Name = $"车辆 [{car?.Code}] 从起点站点 [{taskRecordDto.SrcNodeCode}] 到终点站点 [{taskRecordDto.DestNodeCode}]",
                    Type = TaskInstanceConst.Type.Normal,
                    State = TaskInstanceConst.State.Created,
                    Nodes = [taskRecordDto.SrcNodeId, taskRecordDto.DestNodeId]
                };
                var taskInstanceEntity = Mapper.Map<TaskInstanceEntity>(taskInstanceAdd);
                await UnitOfWork.GetRepository<TaskInstanceEntity>().AddAsync(taskInstanceEntity);
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

        public async Task<int> UpdateTaskRecordWithInstanceAsync(IEnumerable<TaskRecordDto> taskRecordDtos, string state, string workState)
        {
            var result = 0;
            var taskRecordEntities = Mapper.Map<IEnumerable<TaskRecordEntity>>(taskRecordDtos);
            var ids = taskRecordEntities.Select(e => e.Id);
            try
            {
                await UnitOfWork.BeginAsync();
                await UnitOfWork.GetRepository<TaskRecordEntity>().UpdateAsync(taskRecordEntities, e=> e.State!);
                //await UnitOfWork.GetRepository<TaskInstanceEntity>().ExecuteUpdateAsync(e => ids.Contains(e.Id), s => s.SetProperty(b => b.State, state));
                await UnitOfWork.GetRepository<WorkEntity>().ExecuteUpdateAsync(e => ids.Contains(e.TaskId), s => s.SetProperty(b => b.State, workState));
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

        public SemaphoreSlim GetObjectLock()
        {
            return _semaphore;
        }
    }

}