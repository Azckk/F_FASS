using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Entities.DataExtend;

namespace FASS.Service.Services.DataExtend.Interfaces
{
    public interface IPlanRulesService : IAuditService<FrameContext, PlanRulesEntity, PlanRulesDto>
    {

    }
}
