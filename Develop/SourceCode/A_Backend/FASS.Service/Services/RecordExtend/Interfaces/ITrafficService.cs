using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.RecordExtend;
using FASS.Service.Entities.RecordExtend;
using FASS.Service.Models.RecordExtend;

namespace FASS.Service.Services.RecordExtend.Interfaces
{
    public interface ITrafficService : IAuditService<FrameContext, TrafficEntity, TrafficDto>
    {
        int AddEntity(TrafficEntity entity);

        int AddEntities(IEnumerable<TrafficEntity> entities);

        int AddModel(Traffic model);
        int UpdateModel(Traffic model);

        int AddModels(IEnumerable<Traffic> models);
        int DeleteByCarCode(string carCode);

        Task<int> DeleteM3Async();
        Task<int> DeleteM1Async();
        Task<int> DeleteW1Async();
        Task<int> DeleteD1Async();
        Task<int> DeleteAllAsync();
        Task<int> DeleteDayAsync(int day = 90);
    }
}
