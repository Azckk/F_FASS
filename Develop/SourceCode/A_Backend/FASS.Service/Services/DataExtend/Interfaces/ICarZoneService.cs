using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Base;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Entities.DataExtend;

namespace FASS.Service.Services.DataExtend.Interfaces
{
    public interface ICarZoneService : IAuditService<FrameContext, CarZoneEntity, CarZoneDto>
    {
        Task<IPage<CarZoneDto>> ZoneGetPageAsync(string keyValue, Page page);
        Task<int> ZoneAddAsync(string keyValue, IEnumerable<string> zoneIds);
        Task<int> ZoneAddAsync(string keyValue, IEnumerable<ZoneDto> zoneDtos);
        Task<int> ZoneDeleteAsync(string keyValue, IEnumerable<string> carZoneIds);
        Task<int> ZoneDeleteAsync(string keyValue, IEnumerable<CarZoneDto> carZoneDtos);

    }
}
