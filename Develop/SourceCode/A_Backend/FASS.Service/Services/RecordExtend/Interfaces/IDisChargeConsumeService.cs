using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Data.Models.Data;
using FASS.Service.Dtos.RecordExtend;
using FASS.Service.Entities.RecordExtend;

namespace FASS.Service.Services.RecordExtend.Interfaces
{
    public interface IDisChargeConsumeService
        : IAuditService<FrameContext, DisChargeConsumeEntity, DisChargeConsumeDto>
    {
        //Task<int> AddOrUpdateAsync(DisChargeConsumeDto DischargeConsumeDto);
        Task AddOrUpdateAsync(List<Car> cars);
        int UpdateModelsWithOutChargeInstance(IEnumerable<DisChargeConsumeEntity> models);
        int AddModelsWithOutChargeInstance(IEnumerable<DisChargeConsumeEntity> models);
    }
}
