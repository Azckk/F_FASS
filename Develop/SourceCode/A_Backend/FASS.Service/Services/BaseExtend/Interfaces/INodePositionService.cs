using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Base;
using FASS.Data.Entities.Base;

namespace FASS.Service.Services.BaseExtend.Interfaces
{
    public interface INodePositionService : IAuditService<FrameContext, NodePositionEntity, NodePositionDto>
    {

    }
}
