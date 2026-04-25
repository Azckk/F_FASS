using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using DotNetCore.CAP;
using FASS.Data.Dtos.Base;
using FASS.Data.Models.Map;
using FASS.Data.Services.Base.Interfaces;
using FASS.Service.Dtos.BaseExtend;
using FASS.Service.Services.BaseExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Base.Controllers
{
    [Route("api/v1/Base/[controller]/[action]")]
    [Tags("基础管理-站点管理")]
    public class NodeController : BaseController
    {
        private readonly ILogger<NodeController> _logger;
        private readonly INodeService _nodeService;
        private readonly IMapExtendService _mapExtendService;
        private readonly ICapPublisher _capPublisher;
        private readonly AppSettings _appSettings;

        public NodeController(
            ILogger<NodeController> logger,
            INodeService nodeService,
            IMapExtendService mapExtendService,
             ICapPublisher capPublisher,
            AppSettings appSettings)
        {
            _logger = logger;
            _nodeService = nodeService;
            _mapExtendService = mapExtendService;
            _capPublisher = capPublisher;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _nodeService.ToPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ZoneGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _nodeService.ToPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _nodeService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] NodeDto nodeDto)
        {
            await _nodeService.AddOrUpdateAsync(keyValue, nodeDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _nodeService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _nodeService.Set().Where(e => e.IsEnable).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToListAsync();
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Select()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> SelectGetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _nodeService.SelectGetPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetNodePosition([FromQuery] string keyValue)
        {
            var node = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var data = _nodeService.GetNodePosition(node);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetActions([FromQuery] string keyValue)
        {
            var node = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var actions = _nodeService.GetActions(node);
            var actionParams = _nodeService.GetActionParameters(node);
            var data = actions.Select(e =>
            {
                var action = new FASS.Data.Models.Map.Action
                {
                    ActionType = e.ActionType,
                    ActionDescription = e.ActionDescription,
                    BlockingType = e.BlockingType,
                    SortNumber = e.SortNumber,
                    ActionParameters = actionParams.Where(p => p.ActionId == e.Id).Select(p =>
                    {
                        var actionParameter = new ActionParameter
                        {
                            Key = p.Key,
                            Value = p.Value
                        };
                        return actionParameter;
                    }).ToList()
                };
                return action;
            }).ToList();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetExtends([FromQuery] string keyValue)
        {
            var node = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var extends = _nodeService.GetExtends(node);
            var data = extends.Select(e =>
            {
                var extend = new FASS.Data.Models.Map.Extend
                {
                    Key = e.Key,
                    Value = e.Value
                };
                return extend;
            }).ToList();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributes([FromQuery] string keyValue)
        {
            var node = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var attributes = await _mapExtendService.GetNodeAttributes(node);
            var data = attributes.Select(e =>
            {
                var extend = new FASS.Data.Models.Map.Extend
                {
                    Key = e.AttributeType,
                    Value = e.Value
                };
                return extend;
            }).ToList();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlanRules([FromQuery] string keyValue)
        {
            var node = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var planRules = await _mapExtendService.GetNodePlanRules(node);
            return Ok(planRules.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GetMustFree([FromQuery] string keyValue)
        {
            var node = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var nodeMustFree = await _mapExtendService.GetNodeMustFree(node);
            return Ok(nodeMustFree.ToList());
        }

        [HttpPut]
        public async Task<IActionResult> EnableNode([FromQuery] string nodeCode)
        {
            try
            {
                var node = await _nodeService.FirstOrDefaultAsync(e => e.Code == nodeCode);
                if (node == null)
                {
                    return BadRequest($"获取站点失败 [{nodeCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/site/enable/{nodeCode}"
                );
                if (resp.Code == 200)
                {
                    node.IsLock = false;
                    await _nodeService.UpdateAsync(node);
                    await _capPublisher.PublishAsync("NodeController.Enable", new List<string>() { nodeCode });
                    return Ok();
                }
                else
                {
                    return BadRequest($"站点 [{nodeCode}] 启用失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"站点 [{nodeCode}] 启用失败 =>{ex}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> DisableNode([FromQuery] string nodeCode)
        {
            try
            {
                var node = await _nodeService.FirstOrDefaultAsync(e => e.Code == nodeCode);
                if (node == null)
                {
                    return BadRequest($"获取站点失败 [{nodeCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/site/disable/{nodeCode}"
                );
                if (resp.Code == 200)
                {
                    node.IsLock = true;
                    await _nodeService.UpdateAsync(node);
                    await _capPublisher.PublishAsync("NodeController.Disable", new List<string>() { nodeCode });
                    return Ok();
                }
                else
                {
                    return BadRequest($"站点 [{nodeCode}] 启用失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"站点 [{nodeCode}] 禁用失败 =>{ex}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetConflictNodes([FromQuery] string nodeCode)
        {
            var node = await _nodeService.FirstOrDefaultAsync(e => e.Code == nodeCode);
            if (node == null)
            {
                return BadRequest($"获取站点失败 [{nodeCode}]");
            }
            var nodeMustFree = await _mapExtendService.GetConflictNodes(node);
            var data = nodeMustFree.Select(e =>
            {
                return e.NodeCode == nodeCode ? e.MustFreeNodeCode : e.NodeCode;
            }).ToList();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetNodeInfo([FromQuery] string nodeCode)
        {
            var node = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e => e.Code == nodeCode));
            var attributes = await _mapExtendService.GetNodeAttributes(node);
            var nodePosition = _nodeService.GetNodePosition(node);
            var planRules = await _mapExtendService.GetNodePlanRules(node);
            var data = new
            {
                x = nodePosition.X,
                y = nodePosition.Y,
                theta = nodePosition.Theta,
                nodeState = node.IsLock ? "禁用" : "启用",
                planRules = planRules.ToList(),
                attributes = attributes.Select(e =>
                {
                    var attribute = new NodeAttributeDto()
                    {
                        NodeId = e.NodeId,
                        AttributeType = e.AttributeType,
                        Description = e.Description,
                        Value = e.Value
                    };
                    return attribute;
                }).ToList()
            };
            return Ok(data);
        }

    }
}