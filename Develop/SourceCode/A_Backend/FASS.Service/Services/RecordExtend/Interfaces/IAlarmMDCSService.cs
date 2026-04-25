using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Warehouse;
using FASS.Service.Dtos.RecordExtend;
using FASS.Service.Entities.RecordExtend;
using FASS.Service.Models.RecordExtend;

namespace FASS.Service.Services.RecordExtend.Interfaces
{
    public interface IAlarmMdcsService : IAuditService<FrameContext, AlarmMdcsEntity, AlarmMdcsDto>
    {

     
        Task<IEnumerable<AlarmMdcsDto>> GetAlarmMdcsDtos();
        int AddModel(AlarmMdcs model);
        int AddModels(IEnumerable<AlarmMdcs> models);
        int UpdateModel(AlarmMdcs model);
        int UpdateModels(IEnumerable<AlarmMdcs> models);

        Task<int> DeleteM3Async();
        Task<int> DeleteM1Async();
        Task<int> DeleteW1Async();
        Task<int> DeleteD1Async();
        Task<int> DeleteAllAsync();
        Task<int> DeleteDayAsync(int day = 90);
    }
}
