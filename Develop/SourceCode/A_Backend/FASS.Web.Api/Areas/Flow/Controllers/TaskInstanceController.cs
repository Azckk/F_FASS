using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Consts.Flow;
using FASS.Data.Dtos.Flow;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Service.Extends.Flow;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Flow.Controllers
{
    [Route("api/v1/Flow/[controller]/[action]")]
    [Tags("流程管理-任务实例")]
    public class TaskInstanceController : BaseController
    {
        private readonly ILogger<TaskInstanceController> _logger;
        private readonly ITaskInstanceService _taskInstanceService;
        private readonly ITaskTemplateProcessService _taskTemplateProcessService;
        private readonly ICarService _carService;

        public TaskInstanceController(
            ILogger<TaskInstanceController> logger,
            ITaskInstanceService taskInstanceService,
            ITaskTemplateProcessService taskTemplateProcessService,
            ICarService carService)
        {
            _logger = logger;
            _taskInstanceService = taskInstanceService;
            _taskTemplateProcessService = taskTemplateProcessService;
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskInstanceService.ToPageAsync(param);
            var carDtos = await _carService.Set().ToListAsync();
            for (int i = 0; i < page.Data.Count; i++)
            {
                page.Data[i].Remark = carDtos.Where(e => e.Code == page.Data[i].CarCode).FirstOrDefault()?.Name;
                page.Data[i].CarTypeName = carDtos.Where(e => e.Code == page.Data[i].CarCode).FirstOrDefault()?.CarTypeName;
                if ((page.Data[i].Extend ?? "").TryJsonTo<TaskInstanceExtend>(out var extend) && extend != null)
                {
                    page.Data[i].Priority = extend.Priority;
                }
            }
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
            var data = await _taskInstanceService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskInstanceDto taskInstanceDto)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                taskInstanceDto.State = TaskInstanceConst.State.Created;
            }
            else
            {
                var taskInstance = _taskInstanceService.FirstOrDefault(keyValue);
                if (taskInstance == null)
                {
                    return BadRequest("任务实例不存在，无法进行操作");
                }
                if (!TaskInstanceConst.State.Update.Contains(taskInstance.State))
                {
                    return BadRequest($"任务实例：[ {taskInstanceDto.Code} ] 已进入处理队列，无法操作");
                }
                taskInstanceDto.State = taskInstance.State;
            }
            if (taskInstanceDto.Nodes?.Length == 0 && taskInstanceDto.Edges?.Length == 0)
            {
                return BadRequest($"任务创建失败,未指定站点或路线");
            }
            var taskTemplateProcess = await _taskTemplateProcessService.ToListAsync(e => e.TaskTemplateId == taskInstanceDto.TaskTemplateId);
            var existEmpty = false;
            if (taskInstanceDto.Nodes?.Length > 0)
            {
                var sortNodeArray = taskTemplateProcess.OrderBy(e => e.SortNumber).Select(e => e.NodeId).ToArray();
                if (sortNodeArray.Length != taskInstanceDto.Nodes.Length)
                    return BadRequest($"任务创建失败,指定站点与模板站点个数不一致");
                for (var i = 0; i < sortNodeArray.Length; i++)
                {
                    if (string.IsNullOrEmpty(sortNodeArray[i]) && taskInstanceDto.Nodes[i] == "-")
                    {
                        existEmpty = true;
                        break;
                    }
                }
                if (existEmpty)
                {
                    return BadRequest($"任务创建失败,指定站点不替换项【-】对应的子任务未绑定站点");
                }
            }
            else
            {
                var sortEdgeArray = taskTemplateProcess.OrderBy(e => e.SortNumber).Select(e => e.EdgeId).ToArray();
                if (sortEdgeArray.Length != taskInstanceDto.Edges?.Length)
                    return BadRequest($"任务创建失败,指定路线与模板路线个数不一致");
                for (var i = 0; i < sortEdgeArray.Length; i++)
                {
                    if (string.IsNullOrEmpty(sortEdgeArray[i]) && taskInstanceDto.Edges[i] == "-")
                    {
                        existEmpty = true;
                        break;
                    }
                }
                if (existEmpty)
                {
                    return BadRequest($"任务创建失败,指定路线不替换项【-】对应的子任务未绑定路线");
                }
            }
            await _taskInstanceService.AddOrUpdateAsync(keyValue, taskInstanceDto);
            var result = await _taskInstanceService.AnyAsync(e => e.Id == taskInstanceDto.Id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            var dtos = await _taskInstanceService.ToListAsync(e => keyValues.Contains(e.Id) && TaskInstanceConst.State.Delete.Contains(e.State));
            var data = await _taskInstanceService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> ForceDelete([FromBody] List<string> keyValues)
        {
            var data = await _taskInstanceService.DeleteAsync(keyValues);
            return Ok($"操作成功");
        }

        [HttpPut]
        public async Task<IActionResult> Release([FromBody] List<string> keyValues)
        {
            var dtos = await _taskInstanceService.ToListAsync(e => keyValues.Contains(e.Id) && TaskInstanceConst.State.Release.Contains(e.State));
            foreach (var dto in dtos)
            {
                dto.State = TaskInstanceConst.State.Released;
            }
            await _taskInstanceService.UpdateAsync(dtos);
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpPut]
        public async Task<IActionResult> Cancel([FromBody] List<string> keyValues)
        {
            var dtos = await _taskInstanceService.ToListAsync(e => keyValues.Contains(e.Id) && TaskInstanceConst.State.Cancel.Contains(e.State));
            foreach (var dto in dtos)
            {
                dto.State = TaskInstanceConst.State.Canceling;
            }
            await _taskInstanceService.UpdateAsync(dtos);
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpPut]
        public async Task<IActionResult> Pause([FromBody] List<string> keyValues)
        {
            var dtos = await _taskInstanceService.ToListAsync(e => keyValues.Contains(e.Id) && TaskInstanceConst.State.Pause.Contains(e.State));
            foreach (var dto in dtos)
            {
                dto.State = TaskInstanceConst.State.Pausing;
            }
            await _taskInstanceService.UpdateAsync(dtos);
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpPut]
        public async Task<IActionResult> Resume([FromBody] List<string> keyValues)
        {
            var dtos = await _taskInstanceService.ToListAsync(e => keyValues.Contains(e.Id) && TaskInstanceConst.State.Resume.Contains(e.State));
            foreach (var dto in dtos)
            {
                dto.State = TaskInstanceConst.State.Resuming;
            }
            await _taskInstanceService.UpdateAsync(dtos);
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM3()
        {
            var dtos = await _taskInstanceService.ToListAsync(e => e.CreateAt < DateTime.Now.AddMonths(-3) && TaskInstanceConst.State.Delete.Contains(e.State));
            var data = await _taskInstanceService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM1()
        {
            var dtos = await _taskInstanceService.ToListAsync(e => e.CreateAt < DateTime.Now.AddMonths(-1) && TaskInstanceConst.State.Delete.Contains(e.State));
            var data = await _taskInstanceService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteW1()
        {
            var dtos = await _taskInstanceService.ToListAsync(e => e.CreateAt < DateTime.Now.AddDays(-7) && TaskInstanceConst.State.Delete.Contains(e.State));
            var data = await _taskInstanceService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteD1()
        {
            var dtos = await _taskInstanceService.ToListAsync(e => e.CreateAt < DateTime.Now.AddDays(-1) && TaskInstanceConst.State.Delete.Contains(e.State));
            var data = await _taskInstanceService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var dtos = await _taskInstanceService.ToListAsync(e => TaskInstanceConst.State.Delete.Contains(e.State));
            var data = await _taskInstanceService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _taskInstanceService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }
    }
}