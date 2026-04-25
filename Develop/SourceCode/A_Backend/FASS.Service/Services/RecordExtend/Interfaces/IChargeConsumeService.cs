using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.RecordExtend;
using FASS.Service.Entities.RecordExtend;

namespace FASS.Service.Services.RecordExtend.Interfaces
{
    public interface IChargeConsumeService : IAuditService<FrameContext, ChargeConsumeEntity, ChargeConsumeDto>
    {
        Task AddOrUpdateAsync(List<Data.Models.Data.Car> cars);
        int UpdateModelsWithOutChargeInstance(IEnumerable<ChargeConsumeEntity> models);
        int AddModelsWithOutChargeInstance(IEnumerable<ChargeConsumeEntity> models);
    }
}
