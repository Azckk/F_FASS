using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Entities.FlowExtend;

namespace FASS.Service.Services.FlowExtend.Interfaces
{
    public interface ITaskTemplateMdcsService : IAuditService<FrameContext, TaskTemplateMdcsEntity, TaskTemplateMdcsDto>
    {
    }
}
