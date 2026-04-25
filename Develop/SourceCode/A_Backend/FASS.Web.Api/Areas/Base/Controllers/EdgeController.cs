using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Base;
using FASS.Data.Models.Map;
using FASS.Data.Services.Base.Interfaces;
using FASS.Service.Dtos.BaseExtend;
using FASS.Service.Services.BaseExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Base.Controllers
{
    [Route("api/v1/Base/[controller]/[action]")]
    [Tags("基础管理-路线管理")]
    public class EdgeController : BaseController
    {
        private readonly ILogger<EdgeController> _logger;
        private readonly IEdgeService _edgeService;
        private readonly IMapExtendService _mapExtendService;

        public EdgeController(
            ILogger<EdgeController> logger,
            IEdgeService edgeService,
            IMapExtendService mapExtendService)
        {
            _logger = logger;
            _edgeService = edgeService;
            _mapExtendService = mapExtendService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _edgeService.ToPageAsync(param);
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
            var data = await _edgeService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] EdgeDto edgeDto)
        {
            await _edgeService.AddOrUpdateAsync(keyValue, edgeDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _edgeService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _edgeService.Set().Where(e => e.IsEnable).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToListAsync();
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
            var page = await _edgeService.SelectGetPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetTrajectory([FromQuery] string keyValue)
        {
            var node = Guard.NotNull(await _edgeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var trajectory = _edgeService.GetTrajectory(node);
            var controlPoints = _edgeService.GetControlPoints(node);
            var data = new Trajectory();
            data.Degree = trajectory.Degree;
            data.KnotVector = trajectory.KnotVector;
            data.ControlPoints = controlPoints.Select(e =>
            {
                var controlPoint = new ControlPoint();
                controlPoint.X = e.X;
                controlPoint.Y = e.Y;
                controlPoint.Weight = e.Weight;
                return controlPoint;
            }).ToList();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetActions([FromQuery] string keyValue)
        {
            var node = Guard.NotNull(await _edgeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var actions = _edgeService.GetActions(node);
            var actionParams = _edgeService.GetActionParameters(node);
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
            var node = Guard.NotNull(await _edgeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var extends = _edgeService.GetExtends(node);
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
            var edge = Guard.NotNull(await _edgeService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var attributes = await _mapExtendService.GetEdgeAttributes(edge);
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
        public async Task<IActionResult> GetEdgeInfo([FromQuery] string edgeCode)
        {
            var edge = Guard.NotNull(await _edgeService.FirstOrDefaultAsync(e => e.Code == edgeCode));
            var attributes = await _mapExtendService.GetEdgeAttributes(edge);
            var extends = _edgeService.GetExtends(edge).Where(e => e.Key == "ObstacleParams").FirstOrDefault();//获取路线避障参数
            var data = new
            {
                startNodeCode = edge.StartNodeCode,
                endNodeCode = edge.EndNodeCode,
                isOneWay = edge.IsOneway,
                speed = edge.MaxSpeed,
                obstacleParams = extends,
                attributes = attributes.Select(e =>
                {
                    var attribute = new EdgeAttributeDto()
                    {
                        EdgeId = edge.Id,
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