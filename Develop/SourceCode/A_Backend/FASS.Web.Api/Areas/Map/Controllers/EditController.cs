using Common.AspNetCore.Extensions;
using Common.NETCore;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Base;
using FASS.Data.Models.Map;
using FASS.Data.Services.Base.Interfaces;
using FASS.Service.Dtos.BaseExtend;
using FASS.Service.Services.BaseExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FASS.Web.Api.Areas.Map.Controllers
{
    [Route("api/v1/Map/[controller]/[action]")]
    [Tags("地图管理-地图编辑")]
    public class EditController : BaseController
    {
        private readonly ILogger<EditController> _logger;
        private readonly IMapService _mapService;
        private readonly IMapExtendService _mapExtendService;

        public EditController(
            ILogger<EditController> logger,
            IMapService mapService,
            IMapExtendService mapExtendService)
        {
            _logger = logger;
            _mapService = mapService;
            _mapExtendService = mapExtendService;
        }

        [HttpPut]
        public async Task<IActionResult> Save()
        {
            var rawBody = await Request.GetRawBodyStringAsync();
            var panel = Guard.NotNull(rawBody.JsonTo<Panel>());

            var mapDto = new MapDto
            {
                Id = panel.Map.Id,
                Code = "default",
                Name = "默认地图",
                Version = panel.Map.Base.Version,
                FileName = "default.map",
                FileContent = rawBody
            };

            var mapExtendDtos = new List<MapExtendDto>();

            foreach (var extend in panel.Map.Extends)
            {
                var mapExtendDto = new MapExtendDto
                {
                    MapId = mapDto.Id,
                    Key = extend.Key,
                    Value = extend.Value
                };
                mapExtendDtos.Add(mapExtendDto);
            }

            var nodeDtos = new List<NodeDto>();
            var nodePositionDtos = new List<NodePositionDto>();
            var nodeActionDtos = new List<NodeActionDto>();
            var nodeActionParamDtos = new List<NodeActionParameterDto>();
            var nodeExtendDtos = new List<NodeExtendDto>();

            foreach (var node in panel.Nodes)
            {
                var nodeDto = new NodeDto
                {
                    Id = node.Id,
                    Kind = node.Kind,
                    Type = node.Type,
                    Code = node.Code.Text,
                    Name = node.Name.Text,
                    IsLock = node.Lock.Enable,
                    NodeId = node.Data.NodeId,
                    SequenceId = node.Data.SequenceId,
                    NodeDescription = node.Data.NodeDescription,
                    Released = node.Data.Released
                };
                nodeDtos.Add(nodeDto);

                var nodePositionDto = new NodePositionDto
                {
                    NodeId = nodeDto.Id,
                    X = node.Data.NodePosition.X,
                    Y = node.Data.NodePosition.Y,
                    Theta = node.Data.NodePosition.Theta,
                    AllowedDeviationXY = node.Data.NodePosition.AllowedDeviationXY,
                    AllowedDeviationTheta = node.Data.NodePosition.AllowedDeviationTheta,
                    MapId = node.Data.NodePosition.MapId,
                    MapDescription = node.Data.NodePosition.MapDescription,
                };
                nodePositionDtos.Add(nodePositionDto);

                foreach (var action in node.Data.Actions)
                {
                    var nodeActionDto = new NodeActionDto
                    {
                        NodeId = nodeDto.Id,
                        ActionType = action.ActionType,
                        ActionDescription = action.ActionDescription,
                        BlockingType = action.BlockingType,
                        SortNumber = action.SortNumber

                    };
                    nodeActionDtos.Add(nodeActionDto);

                    foreach (var actionParameter in action.ActionParameters)
                    {
                        var nodeActionParamDto = new NodeActionParameterDto
                        {
                            ActionId = nodeActionDto.Id,
                            Key = actionParameter.Key,
                            Value = actionParameter.Value
                        };
                        nodeActionParamDtos.Add(nodeActionParamDto);
                    }
                }

                foreach (var extend in node.Extends)
                {
                    var nodeExtendDto = new NodeExtendDto
                    {
                        NodeId = nodeDto.Id,
                        Key = extend.Key,
                        Value = extend.Value
                    };
                    nodeExtendDtos.Add(nodeExtendDto);
                }
            }

            var edgeDtos = new List<EdgeDto>();
            var trajectoryDtos = new List<TrajectoryDto>();
            var controlPointDtos = new List<ControlPointDto>();
            var edgeActionDtos = new List<EdgeActionDto>();
            var edgeActionParamDtos = new List<EdgeActionParameterDto>();
            var edgeExtendDtos = new List<EdgeExtendDto>();

            foreach (var edge in panel.Edges)
            {
                var edgeDto = new EdgeDto
                {
                    Id = edge.Id,
                    Kind = edge.Kind,
                    Type = edge.Type,
                    Code = edge.Code.Text,
                    Name = edge.Name.Text,
                    IsLock = edge.Lock.Enable,
                    IsOneway = edge.Base.IsOneway,
                    Width = edge.Base.Width,
                    EdgeId = edge.Data.EdgeId,
                    SequenceId = edge.Data.SequenceId,
                    EdgeDescription = edge.Data.EdgeDescription,
                    Released = edge.Data.Released,
                    StartNodeId = edge.Data.StartNodeId,
                    EndNodeId = edge.Data.EndNodeId,
                    MaxSpeed = edge.Data.MaxSpeed,
                    MaxHeight = edge.Data.MaxHeight,
                    MinHeight = edge.Data.MinHeight,
                    Orientation = edge.Data.Orientation,
                    OrientationType = edge.Data.OrientationType,
                    Direction = edge.Data.Direction,
                    RotationAllowed = edge.Data.RotationAllowed,
                    MaxRotationSpeed = edge.Data.MaxRotationSpeed,
                    Length = edge.Data.Length
                };
                edgeDtos.Add(edgeDto);

                var trajectoryDto = new TrajectoryDto
                {
                    EdgeId = edgeDto.Id,
                    Degree = edge.Data.Trajectory.Degree,
                    KnotVector = edge.Data.Trajectory.KnotVector
                };
                trajectoryDtos.Add(trajectoryDto);

                foreach (var controlPoint in edge.Data.Trajectory.ControlPoints)
                {
                    var controlPointDto = new ControlPointDto
                    {
                        TrajectoryId = trajectoryDto.Id,
                        X = controlPoint.X,
                        Y = controlPoint.Y,
                        Weight = controlPoint.Weight
                    };
                    controlPointDtos.Add(controlPointDto);
                }

                foreach (var action in edge.Data.Actions)
                {
                    var edgeActionDto = new EdgeActionDto
                    {
                        EdgeId = edgeDto.Id,
                        ActionType = action.ActionType,
                        ActionDescription = action.ActionDescription,
                        BlockingType = action.BlockingType,
                        SortNumber = action.SortNumber
                    };
                    edgeActionDtos.Add(edgeActionDto);

                    foreach (var actionParameter in action.ActionParameters)
                    {
                        var edgeActionParamDto = new EdgeActionParameterDto
                        {
                            ActionId = edgeActionDto.Id,
                            Key = actionParameter.Key,
                            Value = actionParameter.Value
                        };
                        edgeActionParamDtos.Add(edgeActionParamDto);
                    }
                }

                foreach (var extend in edge.Extends)
                {
                    var edgeExtendDto = new EdgeExtendDto
                    {
                        EdgeId = edgeDto.Id,
                        Key = extend.Key,
                        Value = extend.Value
                    };
                    edgeExtendDtos.Add(edgeExtendDto);
                }
            }

            foreach (var edge in panel.Edges.Where(e => !e.Base.IsOneway))
            {
                var codes = edge.Code.Text.Split('-');
                var edgeDto = new EdgeDto
                {
                    Id = edge.Id,
                    Kind = edge.Kind,
                    Type = edge.Type,
                    Code = $"{codes[1]}-{codes[0]}",
                    Name = edge.Name.Text,
                    IsLock = edge.Lock.Enable,
                    IsOneway = edge.Base.IsOneway,
                    Width = edge.Base.Width,
                    EdgeId = edge.Id,
                    SequenceId = edge.Data.SequenceId,
                    EdgeDescription = edge.Data.EdgeDescription,
                    Released = edge.Data.Released,
                    StartNodeId = edge.Data.EndNodeId,
                    EndNodeId = edge.Data.StartNodeId,
                    MaxSpeed = edge.Data.MaxSpeed,
                    MaxHeight = edge.Data.MaxHeight,
                    MinHeight = edge.Data.MinHeight,
                    Orientation = edge.Data.Orientation,
                    OrientationType = edge.Data.OrientationType,
                    Direction = edge.Data.Direction,
                    RotationAllowed = edge.Data.RotationAllowed,
                    MaxRotationSpeed = edge.Data.MaxRotationSpeed,
                    Length = edge.Data.Length
                };
                edgeDto.Id += "_reverse";
                edgeDto.EdgeId += "_reverse";
                edgeDtos.Add(edgeDto);

                var trajectoryDto = new TrajectoryDto
                {
                    EdgeId = edgeDto.Id,
                    Degree = edge.Data.Trajectory.Degree,
                    KnotVector = edge.Data.Trajectory.KnotVector
                };
                trajectoryDto.Id += "_reverse";
                trajectoryDtos.Add(trajectoryDto);

                foreach (var controlPoint in edge.Data.Trajectory.ControlPoints)
                {
                    var controlPointDto = new ControlPointDto
                    {
                        TrajectoryId = trajectoryDto.Id,
                        X = controlPoint.X,
                        Y = controlPoint.Y,
                        Weight = controlPoint.Weight
                    };
                    controlPointDto.Id += "_reverse";
                    controlPointDtos.Add(controlPointDto);
                }

                foreach (var action in edge.Data.Actions)
                {
                    var edgeActionDto = new EdgeActionDto
                    {
                        EdgeId = edgeDto.Id,
                        ActionType = action.ActionType,
                        ActionDescription = action.ActionDescription,
                        BlockingType = action.BlockingType,
                        SortNumber = action.SortNumber
                    };
                    edgeActionDto.Id += "_reverse";
                    edgeActionDtos.Add(edgeActionDto);

                    foreach (var actionParameter in action.ActionParameters)
                    {
                        var edgeActionParamDto = new EdgeActionParameterDto
                        {
                            ActionId = edgeActionDto.Id,
                            Key = actionParameter.Key,
                            Value = actionParameter.Value
                        };
                        edgeActionParamDto.Id += "_reverse";
                        edgeActionParamDtos.Add(edgeActionParamDto);
                    }
                }

                foreach (var extend in edge.Extends)
                {
                    var edgeExtendDto = new EdgeExtendDto
                    {
                        EdgeId = edgeDto.Id,
                        Key = extend.Key,
                        Value = extend.Value
                    };
                    edgeExtendDto.Id += "_reverse";
                    edgeExtendDtos.Add(edgeExtendDto);
                }
            }

            var zoneDtos = new List<ZoneDto>();
            var zoneNodeDtos = new List<ZoneNodeDto>();
            var zoneExtendDtos = new List<ZoneExtendDto>();

            foreach (var zone in panel.Zones)
            {
                var zoneDto = new ZoneDto
                {
                    Id = zone.Id,
                    Kind = zone.Kind,
                    Type = zone.Type,
                    Code = zone.Code.Text,
                    Name = zone.Name.Text,
                    IsLock = zone.Lock.Enable,
                    MaxCar = zone.Data.MaxCar
                };
                zoneDtos.Add(zoneDto);

                foreach (var nodeId in zone.Data.NodeIds)
                {
                    var zoneNodeDto = new ZoneNodeDto
                    {
                        ZoneId = zone.Id,
                        NodeId = nodeId
                    };
                    zoneNodeDtos.Add(zoneNodeDto);
                }

                foreach (var extend in zone.Extends)
                {
                    var zoneExtendDto = new ZoneExtendDto
                    {
                        ZoneId = zoneDto.Id,
                        Key = extend.Key,
                        Value = extend.Value
                    };
                    zoneExtendDtos.Add(zoneExtendDto);
                }
            }

            _mapService.Save(
                mapDto, mapExtendDtos,
                nodeDtos, nodePositionDtos, nodeActionDtos, nodeActionParamDtos, nodeExtendDtos,
                edgeDtos, trajectoryDtos, controlPointDtos, edgeActionDtos, edgeActionParamDtos, edgeExtendDtos,
                zoneDtos, zoneNodeDtos, zoneExtendDtos);

            var nodeAttributeDtos = new List<NodeAttributeDto>();
            var nodePlanRulesDtos = new List<NodePlanRulesDto>();
            var nodeMustFreeDtos = new List<NodeMustFreeDto>();
            var edgeAttributeDtos = new List<EdgeAttributeDto>();
            var zoneAttributeDtos = new List<ZoneAttributeDto>();
            var zonePlanRulesDtos = new List<ZonePlanRulesDto>();

            foreach (var node in panel.Nodes)
            {
                foreach (var extend in node.Extends)
                {
                    var nodeExtendDto = extend.DeepClone<NodeExtendDto>();
                    if (nodeExtendDto.Key.Equals("Attribute"))
                    {
                        if (nodeExtendDto.Value != null)
                        {
                            var attributes = nodeExtendDto.Value.JsonTo<List<NodeAttributeDto>>();
                            if (attributes != null && attributes.Count > 0)
                            {
                                attributes.ForEach(attr =>
                                {
                                    attr.NodeId = node.Id;
                                });
                                nodeAttributeDtos.AddRange(attributes);
                            }
                        }
                    }
                    else if (nodeExtendDto.Key.Equals("PlanRules"))
                    {
                        if (nodeExtendDto.Value != null)
                        {
                            var planRules = nodeExtendDto.Value.JsonTo<List<NodePlanRulesDto>>();
                            if (planRules != null && planRules.Count > 0)
                            {
                                planRules.ForEach(attr =>
                                {
                                    attr.NodeId = node.Id;
                                });
                                nodePlanRulesDtos.AddRange(planRules);
                            }
                        }
                    }
                    else if (nodeExtendDto.Key.Equals("MustFree"))
                    {
                        if (nodeExtendDto.Value != null)
                        {
                            var mustFreeNodes = Regex.Replace(nodeExtendDto.Value, @"^\[|\]$", "");
                            if (!string.IsNullOrEmpty(mustFreeNodes))
                            {
                                var nodeArr = mustFreeNodes.Split(',');
                                foreach (var nodeId in nodeArr)
                                {
                                    var mustFreeDto = new NodeMustFreeDto()
                                    {
                                        MustFreeNodeCode = nodeId,
                                        NodeCode = node.Code.Text
                                    };
                                    nodeMustFreeDtos.Add(mustFreeDto);
                                }
                            }
                        }

                    }
                    else { }
                }
            }

            foreach (var edge in panel.Edges)
            {
                foreach (var extend in edge.Extends)
                {
                    var edgeExtendDto = extend.DeepClone<EdgeExtendDto>();
                    if (edgeExtendDto.Key.Equals("Attribute"))
                    {
                        if (edgeExtendDto.Value != null)
                        {
                            var attributes = edgeExtendDto.Value.JsonTo<List<EdgeAttributeDto>>();
                            if (attributes != null && attributes.Count > 0)
                            {
                                attributes.ForEach(attr =>
                                {
                                    attr.EdgeId = edge.Id;
                                });
                                edgeAttributeDtos.AddRange(attributes);
                            }
                        }

                    }
                }
            }

            foreach (var edge in panel.Edges.Where(e => !e.Base.IsOneway))
            {
                foreach (var extend in edge.Extends)
                {
                    var edgeExtendDto = extend.DeepClone<EdgeExtendDto>();
                    if (edgeExtendDto.Key.Equals("Attribute"))
                    {
                        if (edgeExtendDto.Value != null)
                        {
                            var attributes = edgeExtendDto.Value.JsonTo<List<EdgeAttributeDto>>();
                            if (attributes != null && attributes.Count > 0)
                            {
                                attributes.ForEach(attr =>
                                {
                                    attr.EdgeId = edge.Id + "_reverse";
                                    attr.Id += "_reverse";
                                });
                                edgeAttributeDtos.AddRange(attributes);
                            }
                        }
                    }
                }
            }

            foreach (var zone in panel.Zones)
            {
                foreach (var extend in zone.Extends)
                {
                    var zoneExtendDto = extend.DeepClone<ZoneExtendDto>();
                    if (zoneExtendDto.Key.Equals("Attribute"))
                    {
                        if (zoneExtendDto.Value != null)
                        {
                            var attributes = zoneExtendDto.Value.JsonTo<List<ZoneAttributeDto>>();
                            if (attributes != null && attributes.Count > 0)
                            {
                                attributes.ForEach(attr =>
                                {
                                    attr.ZoneId = zone.Id;
                                });
                                zoneAttributeDtos.AddRange(attributes);
                            }
                        }
                    }
                    else if (zoneExtendDto.Key.Equals("PlanRules"))
                    {
                        if (zoneExtendDto.Value != null)
                        {
                            var planRules = zoneExtendDto.Value.JsonTo<List<ZonePlanRulesDto>>();
                            if (planRules != null && planRules.Count > 0)
                            {
                                planRules.ForEach(attr =>
                                {
                                    attr.ZoneId = zone.Id;
                                });
                                zonePlanRulesDtos.AddRange(planRules);
                            }
                        }
                    }
                    else { }
                }
            }

            _mapExtendService.Save(
                nodeAttributeDtos, nodePlanRulesDtos, nodeMustFreeDtos,
                edgeAttributeDtos, zoneAttributeDtos, zonePlanRulesDtos, nodeDtos);

            return Ok();
        }

        //[HttpGet]
        //public IActionResult NodePositionForm()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult TrajectoryForm()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ControlPointIndex()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ControlPointForm()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ActionIndex()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ActionForm()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ActionParameterIndex()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ActionParameterForm()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ExtendIndex()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ExtendForm()
        //{
        //    return View();
        //}
    }
}