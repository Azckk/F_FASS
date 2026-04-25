using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.Custom;
using FASS.Service.Entities.Custom;

namespace FASS.Service.Services.Custom.Interfaces
{
    public interface IDemoService : IAuditService<FrameContext, DemoEntity, DemoDto>
    {

    }
}
