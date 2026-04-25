using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.Object;
using FASS.Service.Entities.Object;

namespace FASS.Service.Services.Object.Interfaces
{
    public interface IThirdpartySystemService : IAuditService<FrameContext, ThirdpartySystemEntity, ThirdpartySystemDto>
    {

    }


}
