using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using Common.NETCore.Models;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Data;
using FASS.Data.Services.Data.Interfaces;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Dtos.Warehouse;
using FASS.Service.Models.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Warehouse.Controllers
{
    [Route("api/v1/Warehouse/[controller]/[action]")]
    [Tags("仓储管理-库位可视化")]
    public class VisualizationController : BaseController
    {
        private readonly ILogger<VisualizationController> _logger;
        private readonly IAreaService _areaService;
        private readonly ITaskRecordService _taskRecordService;
        private readonly IStorageService _storageService;
        private readonly IContainerService _containerService;
        private readonly IMaterialService _materialService;
        private readonly ICarService _carService;
        private readonly ITagService _tagService;
        private readonly IStorageTagService _storageTagService;
        private readonly ILogisticsRouteService _logisticsRouteService;
        private readonly AppSettings _appSettings;

        public VisualizationController(
            ILogger<VisualizationController> logger,
            IAreaService areaService,
            ITaskRecordService taskRecordService,
            IStorageService storageService,
            IContainerService containerService,
            IMaterialService materialService,
            ICarService carService,
            ITagService tagService,
            IStorageTagService storageTagService,
            ILogisticsRouteService logisticsRouteService,
            AppSettings appSettings)
        {
            _logger = logger;
            _areaService = areaService;
            _taskRecordService = taskRecordService;
            _storageService = storageService;
            _containerService = containerService;
            _materialService = materialService;
            _carService = carService;
            _tagService = tagService;
            _storageTagService = storageTagService;
            _logisticsRouteService = logisticsRouteService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _areaService.Set().Where(e => e.IsEnable).OrderBy(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> StorageGetPage([FromQuery] string? keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _areaService.StorageGetPageAsync(keyValue, param);
            //获取储位对应的标签
            var storageTagDtos = await _storageTagService.Set().ToListAsync();
            var tagDtos = await _tagService.Set().ToListAsync();
            //获取库位容器对应关系
            var storageContainerDtos = await _storageService.GetStorageContainerListAsync("");
            //获取容器信息
            var containerDtos = await _containerService.Set().ToListAsync();
            //获取容器物料对应关系
            var containerMaterialDtos = await _containerService.GetContainerMaterialListAsync("");
            //获取物料信息
            var materialDtos = await _materialService.Set().ToListAsync();
            for (var i = 0; i < page.Data.Count; i++)
            {
                //获取储位的标签关系
                var tagIds = storageTagDtos.Where(e => e.StorageId == page.Data[i].Id).Select(e => e.TagId).ToList();
                page.Data[i].Tags = tagDtos.Where(e => tagIds.Contains(e.Id)).ToList();
                //获取储位的容器
                var containerIds = storageContainerDtos.Where(e => e.StorageId == page.Data[i].Id).Select(e => e.ContainerId).ToList();
                page.Data[i].Containers = containerDtos.Where(e => containerIds.Contains(e.Id)).ToList();
                //获取容器的物料
                if (containerIds != null && containerIds.Count > 0)
                {
                    var materialIds = containerMaterialDtos.Where(e => containerIds.Contains(e.ContainerId)).Select(e => e.MaterialId).ToList();
                    page.Data[i].Materials = materialDtos.Where(e => materialIds.Contains(e.Id)).ToList();
                }
            }
            var data = new
            {
                rows = page.Data.OrderBy(e => e.Code.Length).ThenBy(e => e.Code),
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddTaskRecord([FromBody] TaskRecordDto taskRecordDto)
        {
            CarDto? car = null;
            if (Guard.NotNull(taskRecordDto.Type).Equals("Template"))
            {
                //模板任务，根据储位id获取对应站点id和code
                var startStorageDto = await _storageService.Set().Where(e => e.Id == taskRecordDto.SrcStorageId).FirstOrDefaultAsync();
                if (startStorageDto == null) return BadRequest($"获取起点库位失败 [{taskRecordDto.SrcStorageId}]");
                var endStorageDto = await _storageService.Set().Where(e => e.Id == taskRecordDto.DestStorageId).FirstOrDefaultAsync();
                if (endStorageDto == null) return BadRequest($"获取终点库位失败 [{taskRecordDto.DestStorageId}]");
                if (!string.IsNullOrEmpty(taskRecordDto.CarId))
                {
                    car = await _carService.FirstOrDefaultAsync(e => e.Id == taskRecordDto.CarId);
                    if (car == null) return BadRequest($"获取车辆失败 [{taskRecordDto.CarId}]");
                }
                taskRecordDto.SrcNodeId = startStorageDto.NodeId;
                taskRecordDto.SrcNodeCode = startStorageDto.NodeCode;
                taskRecordDto.SrcAreaId = startStorageDto.AreaId;
                taskRecordDto.DestNodeId = endStorageDto.NodeId;
                taskRecordDto.DestNodeCode = endStorageDto.NodeCode;
                taskRecordDto.DestAreaId = endStorageDto.AreaId;
                taskRecordDto.Name = $"车辆 [{car?.Code}] 从起点站点 [{taskRecordDto.SrcNodeCode}] 到终点站点 [{taskRecordDto.DestNodeCode}]";
                taskRecordDto.Description = $"{startStorageDto.Code} => {endStorageDto.Code}";
                taskRecordDto.CallMode = "";
            }
            else
            {
                //物流动线 根据物流动线id获取起点/终点库区id
                var logisticsRouteDto = await _logisticsRouteService.Set().Where(e => e.Id == taskRecordDto.TaskTemplateId).FirstOrDefaultAsync();
                if (logisticsRouteDto == null) return BadRequest($"获取物流动线失败 [{taskRecordDto.TaskTemplateId}]");
                //获取起点区域满料占用库位,且未被锁定
                var startStorageDto = await _storageService.Set().Where(e => e.AreaId == logisticsRouteDto.SrcAreaId && e.State == StorageConst.State.FullContainer && !e.IsLock).FirstOrDefaultAsync();
                if (startStorageDto == null) return BadRequest($"物流动线起点区域不存在满料库位 [{logisticsRouteDto.SrcAreaId}]");
                //获取终点区域空库位
                var endStorageDto = await _storageService.Set().Where(e => e.AreaId == logisticsRouteDto.DestAreaId && e.State == StorageConst.State.NoneContainer && !e.IsLock).FirstOrDefaultAsync();
                if (endStorageDto == null) return BadRequest($"物流动线终点区域不存在空库位 [{logisticsRouteDto.DestAreaId}]");
                taskRecordDto.SrcNodeId = startStorageDto.NodeId;
                taskRecordDto.SrcNodeCode = startStorageDto.NodeCode;
                taskRecordDto.DestNodeId = endStorageDto.NodeId;
                taskRecordDto.DestNodeCode = endStorageDto.NodeCode;
                taskRecordDto.SrcStorageId = startStorageDto.Id;
                taskRecordDto.DestStorageId = endStorageDto.Id;
                taskRecordDto.Description = $"{logisticsRouteDto.SrcAreaCode} => {logisticsRouteDto.DestAreaCode}";
                //替换物流动线id,物流动线设置双点任务模板
                taskRecordDto.TaskTemplateId = "Double";
            }
            taskRecordDto.Id = GuidHelper.GetGuidSequential().ToString();
            taskRecordDto.State = TaskRecordConst.State.Created;
            taskRecordDto.Code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            if (string.IsNullOrEmpty(taskRecordDto.Code))
            {
                taskRecordDto.Code = $"{taskRecordDto.SrcNodeCode} => {taskRecordDto.DestNodeCode}";
            }
            var result = await _taskRecordService.AddTaskRecordAsync(taskRecordDto);
            if (result == 0)
            {
                return BadRequest($"任务创建失败");
            }
            var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
            try
            {
                var req = new
                {
                    CarCode = car?.Code,
                    TaskCode = dto.Id,
                    TaskType = dto.TaskTemplateId,
                    Nodes = new List<TaskReqNode>
                    {
                        new TaskReqNode{ Code =Guard.NotNull(taskRecordDto.SrcNodeCode)},
                        new TaskReqNode{ Code =Guard.NotNull(taskRecordDto.DestNodeCode)}
                    }
                };
                var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                );
                if (resp.Code == 200)
                {
                    dto.State = TaskInstanceConst.State.Released;
                    await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                }
                else
                {
                    return BadRequest($"发布任务失败 Code[{resp.Code}], Message[{resp.Message}]");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"发布任务失败 => ex:{ex}");
            }
            return Ok();
        }



        [HttpGet]
        public async Task<IActionResult> TagGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _storageService.TagGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> TagAdd([FromQuery] string keyValue, [FromBody] List<TagDto> tagDtos)
        {
            await _storageService.TagAddAsync(keyValue, tagDtos);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> TagUpdate([FromQuery] string keyValue, [FromBody] List<TagDto> tagDtos)
        {
            await _storageService.TagUpdateAsync(keyValue, tagDtos);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> TagDelete([FromQuery] string keyValue, [FromBody] List<TagDto> tagDtos)
        {
            await _storageService.TagDeleteAsync(keyValue, tagDtos);
            return Ok();
        }

    }

}
