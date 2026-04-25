using Common.NETCore;
using FASS.Boot.Services;
using FASS.Data.Models.Base;
using FASS.Data.Services.Base.Interfaces;
using FASS.Scheduler.Attributes;
using FASS.Scheduler.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Scheduler.Controllers
{
    [AllowAnonymous]
    [TypeFilter(typeof(AuthorizeActionIgonreAttribute))]
    [TypeFilter(typeof(ActionLogIgonreAttribute))]
    [Tags("业务站点接口")]
    public class NodeController : BaseController
    {
        private readonly ILogger<NodeController> _logger;
        private readonly INodeService _nodeService;
        private readonly IBootService _bootService;

        public NodeController(
            ILogger<NodeController> logger,
            INodeService nodeService,
            IBootService bootService
        )
        {
            _logger = logger;
            _nodeService = nodeService;
            _bootService = bootService;
        }

        //[Tags("获取待命点")]
        //[HttpPost]
        //public IActionResult GetStandby(Models.Request.ToSomeWhere request)
        //{
        //    var car = _bootService.Cars.Where(e => e.Code == request.CarCode).FirstOrDefault();
        //    if (car == null)
        //        return BadRequest($"车辆编码不存在 [{request.CarCode}]");
        //    Func<Standby, bool> predicate = e => e.IsEnable;
        //    _bootService.TryGetStandby(car, predicate, out var standbyResult, out var adgesResult);
        //    if (standbyResult == null)
        //        return BadRequest($"未获取到待命点 [{request.CarCode}]");
        //    var response = standbyResult.ToStandby();
        //    return Ok(response);
        //}

        //[Tags("获取充电点")]
        //[HttpPost]
        //public IActionResult GetCharge(Models.Request.ToSomeWhere request)
        //{
        //    var car = _bootService.Cars.Where(e => e.Code == request.CarCode).FirstOrDefault();
        //    if (car == null)
        //        return BadRequest($"车辆编码不存在 [{request.CarCode}]");
        //    Func<Charge, bool> predicate = e => e.IsEnable;
        //    _bootService.TryGetCharge(car, predicate, out var chargeResult, out var adgesResult);
        //    if (chargeResult == null)
        //        return BadRequest($"未获取到充电点 [{request.CarCode}]");
        //    var response = chargeResult.ToCharge();
        //    return Ok(response);
        //}

        //[Tags("获取避让点")]
        //[HttpPost]
        //public IActionResult GetAvoid(Models.Request.ToSomeWhere request)
        //{
        //    var car = _bootService.Cars.Where(e => e.Code == request.CarCode).FirstOrDefault();
        //    if (car == null)
        //        return BadRequest($"车辆编码不存在 [{request.CarCode}]");
        //    Func<Avoid, bool> predicate = e => e.IsEnable;
        //    _bootService.TryGetAvoid(car, predicate, out var avoidResult, out var adgesResult);
        //    if (avoidResult == null)
        //        return BadRequest($"未获取到避让点 [{request.CarCode}]");
        //    var response = avoidResult.ToAvoid();
        //    return Ok(response);
        //}

        [Tags("设置站点禁用")]
        [HttpPost]
        public IActionResult NodeDisable(Models.Request.NodeSetting request)
        {
            if (request.IsEnable)
            {
                return BadRequest($"禁用站点参数错误 [{request.NodeCode},{request.IsEnable}]");
            }
            var node = _bootService.Nodes.FirstOrDefault(e => e.Code == request.NodeCode);
            if (node == null)
            {
                return BadRequest($"获取站点失败 [{request.NodeCode}]");
            }
            Guard.NotNull(_bootService.Nodes.FirstOrDefault(e => e.Code == request.NodeCode)).IsLock = true;
            _nodeService.Repository.ExecuteUpdate(
                e => e.Id == node.Id,
                s => s.SetProperty(b => b.IsLock, true)
            );
            return Ok();
        }

        [Tags("设置站点启用")]
        [HttpPost]
        public IActionResult NodeEnable(Models.Request.NodeSetting request)
        {
            if (!request.IsEnable)
            {
                return BadRequest($"启用站点参数错误 [{request.NodeCode},{request.IsEnable}]");
            }
            var node = _bootService.Nodes.FirstOrDefault(e => e.Code == request.NodeCode);
            if (node == null)
            {
                return BadRequest($"获取站点失败 [{request.NodeCode}]");
            }
            Guard.NotNull(_bootService.Nodes.FirstOrDefault(e => e.Code == request.NodeCode)).IsLock = false;
            _nodeService.Repository.ExecuteUpdate(
                e => e.Id == node.Id,
                s => s.SetProperty(b => b.IsLock, false)
            );
            return Ok();
        }

        [Tags("批量设置站点禁用")]
        [HttpPost]
        public IActionResult NodeListDisable([FromBody] List<int> nodes)
        {
            try
            {
                List<Node> nodesToEnable;
                List<Node> nodesToDisable;
                if (nodes == null || nodes.Count == 0)
                {
                    nodesToEnable = _bootService.Nodes.Where(e => e.IsLock).ToList();
                    nodesToDisable = new List<Node>();
                }
                else
                {
                    List<string> stringNodes = nodes.Select(n => n.ToString()).ToList();
                    // 获取需要启用的节点（当前锁定且不在传入列表中的节点）
                    nodesToEnable = _bootService
                        .Nodes.Where(e => e.IsLock && !stringNodes.Contains(e.Code))
                        .ToList();
                    // 获取需要禁用的节点（当前未锁定且在传入列表中的节点）
                    nodesToDisable = _bootService
                        .Nodes.Where(e => !e.IsLock && stringNodes.Contains(e.Code))
                        .ToList();
                }
                // 启用节点
                foreach (var node in nodesToEnable)
                {
                    _nodeService.Repository.ExecuteUpdate(
                        e => e.Id == node.Id,
                        s => s.SetProperty(b => b.IsLock, false)
                    );
                    node.IsLock = false;
                }

                // 禁用节点
                foreach (var node in nodesToDisable)
                {
                    _nodeService.Repository.ExecuteUpdate(
                        e => e.Id == node.Id,
                        s => s.SetProperty(b => b.IsLock, true)
                    );
                    node.IsLock = true;
                }

                // 检查是否有任何操作
                if (nodesToEnable.Count == 0 && nodesToDisable.Count == 0)
                {
                    return BadRequest("无节点状态需要更新。");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"禁用站点失败: {e.Message}");
            }

            return Ok("节点状态更新成功。");
        }
    }
}
