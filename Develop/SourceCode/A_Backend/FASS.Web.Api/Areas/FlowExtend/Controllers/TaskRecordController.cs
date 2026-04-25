using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using DotNetCore.CAP;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Data;
using FASS.Data.Services.Base.Interfaces;
using FASS.Data.Services.Data.Interfaces;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Extends.Flow;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.FlowExtend.Controllers
{
    [Route("api/v1/Flow/[controller]/[action]")]
    [Tags("流程管理-任务记录(Mdcs)")]
    public class TaskRecordController : BaseController
    {
        private readonly ILogger<TaskRecordController> _logger;
        private readonly ITaskRecordService _taskRecordService;
        private readonly INodeService _nodeService;
        private readonly AppSettings _appSettings;
        private readonly ICarTypeService _carTypeService;
        private readonly ICapPublisher _capPublisher;
        private readonly ICarService _carService;

        public TaskRecordController(
            ILogger<TaskRecordController> logger,
            ITaskRecordService taskRecordService,
            INodeService nodeService,
            ICarService carService,
            ICarTypeService carTypeService,
            ICapPublisher capPublisher,
            AppSettings appSettings
        )
        {
            _logger = logger;
            _taskRecordService = taskRecordService;
            _nodeService = nodeService;
            _carService = carService;
            _carTypeService = carTypeService;
            _capPublisher = capPublisher;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskRecordService.ToPageAsync(param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _taskRecordService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskRecordDto taskRecordDto)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                taskRecordDto.State = TaskRecordConst.State.Created;
            }
            else
            {
                var taskRecord = await _taskRecordService.FirstOrDefaultAsync(keyValue);
                if (taskRecord == null)
                {
                    return BadRequest("任务记录不存在，无法进行操作");
                }
                if (!TaskRecordConst.State.Update.Contains(taskRecord.State))
                {
                    return BadRequest($"任务：[ {taskRecordDto.Code} ] 已进入处理队列，无法操作");
                }
                taskRecordDto.State = taskRecord.State;
            }
            var startNode = await _nodeService.FirstOrDefaultAsync(e =>
                e.Id == taskRecordDto.SrcNodeId
            );
            if (startNode == null)
            {
                return BadRequest($"获取起点站点失败 [{taskRecordDto.SrcNodeId}]");
            }
            var endNode = await _nodeService.FirstOrDefaultAsync(e =>
                e.Id == taskRecordDto.DestNodeId
            );
            if (endNode == null)
            {
                return BadRequest($"获取终点站点失败 [{taskRecordDto.DestNodeId}]");
            }
            taskRecordDto.SrcNodeCode = startNode.Code;
            taskRecordDto.DestNodeCode = endNode.Code;
            taskRecordDto.CallMode = "";
            await _taskRecordService.AddOrUpdateAsync(keyValue, taskRecordDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                keyValues.Contains(e.Id) && TaskRecordConst.State.Delete.Contains(e.State)
            );
            var data = await _taskRecordService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> ForceDelete([FromBody] List<string> keyValues)
        {
            var data = await _taskRecordService.DeleteAsync(keyValues);
            return Ok($"操作成功");
        }

        [HttpPut]
        public async Task<IActionResult> Release([FromBody] List<string> keyValues)
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                keyValues.Contains(e.Id) && (TaskRecordConst.State.Release.Contains(e.State) || (e.State == TaskRecordConst.State.Released && e.CarId == null))
            );//发布状态未选中车的任务可手动发布
            var carTypeDtos = await _carTypeService.Set().ToListAsync();
            int count = 0;
            foreach (var dto in dtos)
            {
                try
                {
                    CarDto carDto = null!;
                    CarTypeDto carTypeDto = null!;
                    if (!string.IsNullOrEmpty(dto.CarId))
                    {
                        carDto = Guard.NotNull(await _carService.FirstOrDefaultAsync(e => e.Id == dto.CarId));
                    }
                    if (!string.IsNullOrEmpty(dto.CarTypeId))
                    {
                        carTypeDto = Guard.NotNull(await _carTypeService.FirstOrDefaultAsync(e => e.Id == dto.CarTypeId));
                    }
                    var srcNode = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e =>
                        e.Id == dto.SrcNodeId
                    ));
                    var destNode = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e =>
                        e.Id == dto.DestNodeId
                    ));
                    var extend = (dto.Extend ?? "").JsonTo<TaskRecordExtend>();
                    var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
                    var containerSize = Models.Pc.Extensions.CarTaskExtension.ToContainerSize(extend != null ? extend.ContainerSize.Length : -1, extend != null ? extend.ContainerSize.Width : -1, extend != null ? extend.ContainerSize.Height : -1);
                    var startNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = srcNode.Code
                    };
                    var endNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = destNode.Code
                    };
                    var req = Models.Pc.Extensions.CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, carDto, carTypeCode);
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/carTask",
                        req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskInstanceConst.State.Released;
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"发布任务失败 => ex:{ex}");
                }
            }
            await _taskRecordService.UpdateAsync(dtos);
            return Ok($"操作成功 [{count}]条,失败[{dtos.Count - count}]条");
        }

        [HttpPut]
        public async Task<IActionResult> Cancel([FromBody] List<string> keyValues)
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                keyValues.Contains(e.Id) && TaskRecordConst.State.Cancel.Contains(e.State)
            );
            int count = 0;
            foreach (var dto in dtos)
            {
                try
                {
                    var req = new { TaskCode = dto.Id };
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/cancelTask",
                        req
                    );
                    if (resp.Code == 200)
                    {
                        await _capPublisher.PublishAsync("TaskInstanceController.Cancel", new TaskInstanceParam
                        {
                            TaskInstanceId = dto.Id,
                            CarId = dto.CarId
                        });
                        dto.State = TaskRecordConst.State.Canceled;
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"取消任务失败 => ex:{ex}");
                }
            }
            //await _taskRecordService.UpdateAsync(dtos);
            if (count > 0)
            {
                await _taskRecordService.UpdateTaskRecordWithInstanceAsync(dtos, TaskInstanceConst.State.Canceling, WorkConst.State.Canceled);
            }
            return Ok($"操作成功 [{count}]条,失败[{dtos.Count - count}]条");
        }

        [HttpPut]
        public async Task<IActionResult> Pause([FromBody] List<string> keyValues)
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                keyValues.Contains(e.Id) && TaskRecordConst.State.Pause.Contains(e.State)
            );//只有发布未分配车的任务可以暂停
            if (dtos.Count != keyValues.Count)
            {
                return BadRequest("记录选择错误! 发布状态的任务才能执行暂停操作!");
            }
            int count = 0;
            foreach (var dto in dtos)
            {
                try
                {
                    await _capPublisher.PublishAsync("TaskInstanceController.Pause", new TaskInstanceParam
                    {
                        TaskInstanceId = dto.Id,
                        CarId = dto.CarId
                    });
                    var req = new { TaskCode = dto.Id, TaskStatus = 1 };
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/stopOrPauseTask",
                        req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskRecordConst.State.Pausing;
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"暂停任务失败 => ex:{ex}");
                }
            }
            //await _taskRecordService.UpdateAsync(dtos);
            await _taskRecordService.UpdateTaskRecordWithInstanceAsync(dtos, TaskInstanceConst.State.Pausing, WorkConst.State.Pausing);
            return Ok($"操作成功 [{count}]条,失败[{dtos.Count - count}]条");
        }

        [HttpPut]
        public async Task<IActionResult> Resume([FromBody] List<string> keyValues)
        {
            try
            {
                int count = 0;
                var dtos = await _taskRecordService.ToListAsync(e =>
                    keyValues.Contains(e.Id) && TaskRecordConst.State.Resume.Contains(e.State)
                );
                if (dtos.Count != keyValues.Count)
                {
                    return BadRequest("记录选择错误! 已暂停的任务才能执行恢复操作!");
                }
                foreach (var dto in dtos)
                {
                    await _capPublisher.PublishAsync("TaskInstanceController.Resume", new TaskInstanceParam
                    {
                        TaskInstanceId = dto.Id,
                        CarId = dto.CarId
                    });
                    var req = new { TaskCode = dto.Id, TaskStatus = 0 };
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/stopOrPauseTask",
                        req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskRecordConst.State.Resuming;
                        count++;
                    }
                }
                //await _taskRecordService.UpdateAsync(dtos);
                await _taskRecordService.UpdateTaskRecordWithInstanceAsync(dtos, TaskInstanceConst.State.Resuming, WorkConst.State.Resuming);
                return Ok($"操作成功 [{dtos.Count}]");
            }
            catch (Exception e)
            {
                return BadRequest($"恢复失败：{e.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Resend([FromBody] List<string> keyValues)
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                keyValues.Contains(e.Id) && TaskRecordConst.State.Resend.Contains(e.State)
            );
            int count = 0;
            foreach (var dto in dtos)
            {
                try
                {
                    TaskRecordDto taskRecordDto = new TaskRecordDto
                    {
                        TaskTemplateId = dto.TaskTemplateId,
                        CarTypeId = dto.CarTypeId,
                        Code = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        Name = dto.Name,
                        Type = dto.Type,
                        SrcNodeId = dto.SrcNodeId,
                        DestNodeId = dto.DestNodeId,
                        Priority = dto.Priority,
                        State = TaskRecordConst.State.Created,
                        Description = dto.Description,
                        IsLoop = dto.IsLoop
                    };
                    var startNode = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e =>
                        e.Id == dto.SrcNodeId
                    ));
                    var endNode = Guard.NotNull(await _nodeService.FirstOrDefaultAsync(e =>
                        e.Id == dto.DestNodeId
                    ));
                    taskRecordDto.SrcNodeCode = startNode.Code;
                    taskRecordDto.DestNodeCode = endNode.Code;
                    await _taskRecordService.AddOrUpdateAsync("", taskRecordDto);
                    count++;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"重发任务失败 => ex:{ex}");
                }
            }
            return Ok($"操作成功 [{count}]条,失败[{dtos.Count - count}]条");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM3()
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                e.CreateAt < DateTime.Now.AddMonths(-3)
                && TaskRecordConst.State.Delete.Contains(e.State)
            );
            var data = await _taskRecordService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM1()
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                e.CreateAt < DateTime.Now.AddMonths(-1)
                && TaskRecordConst.State.Delete.Contains(e.State)
            );
            var data = await _taskRecordService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteW1()
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                e.CreateAt < DateTime.Now.AddDays(-7)
                && TaskRecordConst.State.Delete.Contains(e.State)
            );
            var data = await _taskRecordService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteD1()
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                e.CreateAt < DateTime.Now.AddDays(-1)
                && TaskRecordConst.State.Delete.Contains(e.State)
            );
            var data = await _taskRecordService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var dtos = await _taskRecordService.ToListAsync(e =>
                TaskRecordConst.State.Delete.Contains(e.State)
            );
            var data = await _taskRecordService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _taskRecordService
                .Set()
                .Where(e => e.IsEnable)
                .OrderByDescending(e => e.CreateAt)
                .ToListAsync();
            return Ok(data);
        }
    }
}
