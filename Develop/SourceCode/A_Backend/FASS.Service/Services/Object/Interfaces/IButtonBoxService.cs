using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.Object;
using FASS.Service.Entities.Object;


namespace FASS.Service.Services.Object.Interfaces
{
    public interface IButtonBoxService : IAuditService<FrameContext, ButtonBoxEntity, ButtonBoxDto>
    {
       Task<IPage<ButtonBoxItemDto>> ItemsGetPageAsync(string keyValue, Page page);
    }
}
