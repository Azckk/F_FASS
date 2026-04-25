using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.Object;
using FASS.Service.Entities.Object;


namespace FASS.Service.Services.Object.Interfaces
{
    public interface ITrafficLightService : IAuditService<FrameContext, TrafficLightEntity, TrafficLightDto>
    {
       Task<IPage<TrafficLightItemDto>> ItemsGetPageAsync(string keyValue, Page page);
    }
}
