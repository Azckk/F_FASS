using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Entities.FlowExtend;
using FASS.Service.Models.FlowExtend;

namespace FASS.Service.Services.FlowExtend.Interfaces
{
    public interface ITaskRecordService : IAuditService<FrameContext, TaskRecordEntity, TaskRecordDto>
    {
        //IEnumerable<TaskRecordEntity> GetEntities(Expression<Func<TaskRecordEntity, bool>> whereExpression);
        //TaskRecordEntity GetEntity(Expression<Func<TaskRecordEntity, bool>> whereExpression);
        //int UpdateEntities(IEnumerable<TaskRecordEntity> entities);
        //int UpdateEntity(TaskRecordEntity entity);
        //IEnumerable<TaskRecord> GetModels(Expression<Func<TaskRecord, bool>> whereExpression);
        //TaskRecord GetModel(Expression<Func<TaskRecord, bool>> whereExpression);
        //int DistributeModels(IEnumerable<TaskRecord> models);
        //int DistributeModel(TaskRecord model);
        int UpdateModelsWithOutTaskInstance(IEnumerable<TaskRecord> models);
        int UpdateModelWithOutTaskInstance(TaskRecord model);
        int AddModelsWithOutTaskInstance(IEnumerable<TaskRecord> models);
        int AddModelWithOutTaskInstance(TaskRecord model);
        new Task<int> AddOrUpdateAsync(string? keyValue, TaskRecordDto taskRecordDto);
        new Task<int> DeleteAsync(IEnumerable<string> keyValues);
        Task<int> DeleteM3Async(string? type = null);
        Task<int> DeleteM1Async(string? type = null);
        Task<int> DeleteW1Async(string? type = null);
        Task<int> DeleteD1Async(string? type = null);
        Task<int> DeleteAllAsync(string? type = null);
        Task<int> DeleteDayAsync(string? type = null, int day = 90);

        int AddTaskRecord(TaskRecordDto taskRecordDto, string taskTemplateCode);

        int UpdateTaskRecordState(string keyValue, string state);
        Task<int> UpdateTaskRecordStateAsync(string keyValue, string state);

        Task<int> AddTaskRecordAsync(TaskRecordDto taskRecordDto);

        Task<int> UpdateTaskRecordWithInstanceAsync(IEnumerable<TaskRecordDto> taskRecordDtos, string state, string workState);

        SemaphoreSlim GetObjectLock();

    }
}
