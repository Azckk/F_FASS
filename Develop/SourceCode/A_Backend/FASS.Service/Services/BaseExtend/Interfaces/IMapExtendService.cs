using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Data.Dtos.Base;
using FASS.Data.Entities.Base;
using FASS.Service.Dtos.BaseExtend;

namespace FASS.Service.Services.BaseExtend.Interfaces
{
    public interface IMapExtendService : IAuditService<FrameContext, MapEntity, MapDto>
    {
        int Save(
            IEnumerable<NodeAttributeDto> nodeAttributeDtos,
            IEnumerable<NodePlanRulesDto> nodePlanRulesDtos,
            IEnumerable<NodeMustFreeDto> nodeMustFreeDtos,
            IEnumerable<EdgeAttributeDto> edgeAttributeDtos,
            IEnumerable<ZoneAttributeDto> zoneAttributeDtos,
            IEnumerable<ZonePlanRulesDto> zonePlanRulesDtos,
            IEnumerable<NodeDto> nodeDtos);

        Task<IEnumerable<NodeAttributeDto>> GetNodeAttributes(NodeDto nodeDto);

        Task<IEnumerable<NodePlanRulesDto>> GetNodePlanRules(NodeDto nodeDto);

        Task<IEnumerable<NodeMustFreeDto>> GetNodeMustFree(NodeDto nodeDto);

        Task<IEnumerable<EdgeAttributeDto>> GetEdgeAttributes(EdgeDto edgeDto);

        Task<IEnumerable<ZoneAttributeDto>> GetZoneAttributes(ZoneDto zoneDto);

        Task<IEnumerable<ZonePlanRulesDto>> GetZonePlanRules(ZoneDto zoneDto);
        Task<IEnumerable<NodeMustFreeDto>> GetConflictNodes(NodeDto nodeDto);
        Task<IEnumerable<MapExtendDto>> GetMapExtends(MapDto mapDto);
    }
}
