using AutoMapper;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Data.Dtos.Base;
using FASS.Data.Entities.Base;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Dtos.BaseExtend;
using FASS.Service.Entities.BaseExtend;
using FASS.Service.Services.BaseExtend.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FASS.Service.Services.BaseExtend
{
    public class MapExtendService : AuditService<FrameContext, MapEntity, MapDto>, IMapExtendService
    {
        public MapExtendService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, MapEntity> repository,
            IMapper mapper,
            IValidator<MapDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }


        public int Save(
            IEnumerable<NodeAttributeDto> nodeAttributeDtos,
            IEnumerable<NodePlanRulesDto> nodePlanRulesDtos,
            IEnumerable<NodeMustFreeDto> nodeMustFreeDtos,
            IEnumerable<EdgeAttributeDto> edgeAttributeDtos,
            IEnumerable<ZoneAttributeDto> zoneAttributeDtos,
            IEnumerable<ZonePlanRulesDto> zonePlanRulesDtos,
            IEnumerable<NodeDto> nodeDtos)
        {
            var nodeAttributeEntities = Mapper.Map<IEnumerable<NodeAttributeEntity>>(nodeAttributeDtos);
            var nodePlanRulesEntities = Mapper.Map<IEnumerable<NodePlanRulesEntity>>(nodePlanRulesDtos);
            var nodeMustFreeEntities = Mapper.Map<IEnumerable<NodeMustFreeEntity>>(nodeMustFreeDtos);

            var edgeAttributeEntities = Mapper.Map<IEnumerable<EdgeAttributeEntity>>(edgeAttributeDtos);

            var zoneAttributeEntities = Mapper.Map<IEnumerable<ZoneAttributeEntity>>(zoneAttributeDtos);
            var zonePlanRulesEntities = Mapper.Map<IEnumerable<ZonePlanRulesEntity>>(zonePlanRulesDtos);

            var nodeEntities = Mapper.Map<IEnumerable<NodeDto>>(nodeDtos);//所有站点
            try
            {
                UnitOfWork.Begin();

                UnitOfWork.GetRepository<NodeAttributeEntity>().ExecuteDelete(e => true);
                UnitOfWork.GetRepository<NodePlanRulesEntity>().ExecuteDelete(e => true);
                UnitOfWork.GetRepository<NodeMustFreeEntity>().ExecuteDelete(e => true);

                UnitOfWork.GetRepository<EdgeAttributeEntity>().ExecuteDelete(e => true);

                UnitOfWork.GetRepository<ZoneAttributeEntity>().ExecuteDelete(e => true);
                UnitOfWork.GetRepository<ZonePlanRulesEntity>().ExecuteDelete(e => true);


                UnitOfWork.GetRepository<NodeAttributeEntity>().Add(nodeAttributeEntities);
                UnitOfWork.GetRepository<NodePlanRulesEntity>().Add(nodePlanRulesEntities);
                UnitOfWork.GetRepository<NodeMustFreeEntity>().Add(nodeMustFreeEntities);

                UnitOfWork.GetRepository<EdgeAttributeEntity>().Add(edgeAttributeEntities);

                UnitOfWork.GetRepository<ZoneAttributeEntity>().Add(zoneAttributeEntities);
                UnitOfWork.GetRepository<ZonePlanRulesEntity>().Add(zonePlanRulesEntities);

                //更新库位绑定的站点信息
                var storageEntities = UnitOfWork.GetRepository<StorageEntity>().ToList(e => e.IsEnable);
                var storageEntityList = new List<StorageEntity>();//待更新的库位
                foreach (var storageEntity in storageEntities)
                {
                    if (nodeEntities.Any(e => e.Code == storageEntity.NodeCode))
                    {
                        //匹配到站点
                        storageEntity.NodeId = nodeEntities.Where(e => e.Code == storageEntity.NodeCode).FirstOrDefault()?.Id ?? "NULL";
                    }
                    else
                    {
                        //没有匹配到站点
                        storageEntity.IsEnable = false;
                        storageEntity.NodeCode = "";
                        storageEntity.NodeId = "";
                    }
                    storageEntityList.Add(storageEntity);
                }
                if (storageEntityList.Count > 0)
                {
                    UnitOfWork.GetRepository<StorageEntity>().Update(storageEntityList);
                }
                return UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<NodeAttributeDto>> GetNodeAttributes(NodeDto nodeDto)
        {
            var nodeEntity = Mapper.Map<NodeEntity>(nodeDto);
            var nodeAttributeEntities = await Repository.DbContext.Set<NodeAttributeEntity>()
                .AsNoTracking()
                .Where(e => e.NodeId == nodeEntity.Id)
                .ToListAsync();
            var nodeAttributeDtos = Mapper.Map<IEnumerable<NodeAttributeDto>>(nodeAttributeEntities);
            return nodeAttributeDtos;
        }

        public async Task<IEnumerable<NodePlanRulesDto>> GetNodePlanRules(NodeDto nodeDto)
        {
            var nodeEntity = Mapper.Map<NodeEntity>(nodeDto);
            var nodePlanRulesEntities = await Repository.DbContext.Set<NodePlanRulesEntity>()
                .AsNoTracking()
                .Where(e => e.NodeId == nodeEntity.Id)
                .ToListAsync();
            var nodePlanRulesDtos = Mapper.Map<IEnumerable<NodePlanRulesDto>>(nodePlanRulesEntities);
            return nodePlanRulesDtos;
        }

        public async Task<IEnumerable<NodeMustFreeDto>> GetNodeMustFree(NodeDto nodeDto)
        {
            var nodeEntity = Mapper.Map<NodeEntity>(nodeDto);
            var nodeMustFreeEntities = await Repository.DbContext.Set<NodeMustFreeEntity>()
                .AsNoTracking()
                .Where(e => e.NodeCode == nodeEntity.Code)
                .ToListAsync();
            var nodeMustFreeDtos = Mapper.Map<IEnumerable<NodeMustFreeDto>>(nodeMustFreeEntities);
            return nodeMustFreeDtos;
        }

        public async Task<IEnumerable<EdgeAttributeDto>> GetEdgeAttributes(EdgeDto edgeDto)
        {
            var edgeEntity = Mapper.Map<EdgeEntity>(edgeDto);
            var edgeAttributeEntities = await Repository.DbContext.Set<EdgeAttributeEntity>()
                .AsNoTracking()
                .Where(e => e.EdgeId == edgeEntity.Id)
                .ToListAsync();
            var edgeAttributeDtos = Mapper.Map<IEnumerable<EdgeAttributeDto>>(edgeAttributeEntities);
            return edgeAttributeDtos;
        }

        public async Task<IEnumerable<ZoneAttributeDto>> GetZoneAttributes(ZoneDto zoneDto)
        {
            var zoneEntity = Mapper.Map<ZoneEntity>(zoneDto);
            var zoneAttributeEntities = await Repository.DbContext.Set<ZoneAttributeEntity>()
                .AsNoTracking()
                .Where(e => e.ZoneId == zoneEntity.Id)
                .ToListAsync();
            var zoneAttributeDtos = Mapper.Map<IEnumerable<ZoneAttributeDto>>(zoneAttributeEntities);
            return zoneAttributeDtos;
        }

        public async Task<IEnumerable<ZonePlanRulesDto>> GetZonePlanRules(ZoneDto zoneDto)
        {
            var zoneEntity = Mapper.Map<ZoneEntity>(zoneDto);
            var zonePlanRulesEntities = await Repository.DbContext.Set<ZonePlanRulesEntity>()
                .AsNoTracking()
                .Where(e => e.ZoneId == zoneEntity.Id)
                .ToListAsync();
            var zonePlanRulesDtos = Mapper.Map<IEnumerable<ZonePlanRulesDto>>(zonePlanRulesEntities);
            return zonePlanRulesDtos;
        }

        public async Task<IEnumerable<NodeMustFreeDto>> GetConflictNodes(NodeDto nodeDto)
        {
            var nodeEntity = Mapper.Map<NodeEntity>(nodeDto);
            var nodeMustFreeEntities = await Repository.DbContext.Set<NodeMustFreeEntity>()
                .AsNoTracking()
                .Where(e => e.NodeCode == nodeEntity.Code || e.MustFreeNodeCode == nodeEntity.Code)
                .ToListAsync();
            var nodeMustFreeDtos = Mapper.Map<IEnumerable<NodeMustFreeDto>>(nodeMustFreeEntities);
            return nodeMustFreeDtos;
        }

        public async Task<IEnumerable<MapExtendDto>> GetMapExtends(MapDto mapDto)
        {
            var mapEntity = Mapper.Map<MapEntity>(mapDto);
            var mapExtendEntities = await Repository.Set()
                .AsNoTracking()
                .Where(e => e == mapEntity)
                .SelectMany(e => e.MapExtends)
                .ToListAsync();
            var mapExtendDtos = Mapper.Map<IEnumerable<MapExtendDto>>(mapExtendEntities);
            return mapExtendDtos;
        }

    }
}
