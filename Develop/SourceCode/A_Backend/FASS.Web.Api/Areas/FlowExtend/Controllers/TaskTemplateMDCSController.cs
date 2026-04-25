using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Services.Data.Interfaces;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.FlowExtend.Controllers
{

    [Route("api/v1/Flow/[controller]/[action]")]
    [Tags("流程管理-任务模板(Mdcs)")]
    public class TaskTemplateMdcsController : BaseController
    {
        private readonly ILogger<TaskTemplateMdcsController> _logger;
        private readonly ICarTypeService _carTypeService;
        private readonly ITaskTemplateMdcsService _taskTemplateMdcsService;

        public TaskTemplateMdcsController(
            ILogger<TaskTemplateMdcsController> logger,
            ICarTypeService carService,
            ITaskTemplateMdcsService taskTemplateMdcsService)
        {
            _logger = logger;
            _carTypeService = carService;
            _taskTemplateMdcsService = taskTemplateMdcsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskTemplateMdcsService.ToPageAsync(param);
            List<TaskTemplateMdcsDto> list = new List<TaskTemplateMdcsDto>();
            var carTypeDtos = await _carTypeService.ToListAsync(e => true);
            foreach (var item in page.Data.ToList())
            {
                var dto = item.DeepClone();
                var carTypeDto = carTypeDtos.FirstOrDefault(e => e.Id == dto.CarTypeId);
                dto.CarTypeCode = carTypeDto?.Code;
                dto.CarTypeName = carTypeDto?.Name;
                list.Add(dto);
            }
            var data = new
            {
                rows = list,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _taskTemplateMdcsService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskTemplateMdcsDto taskTemplateMdcsDto)
        {
            await _taskTemplateMdcsService.AddOrUpdateAsync(keyValue, taskTemplateMdcsDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _taskTemplateMdcsService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _taskTemplateMdcsService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetListByTypeToSelect([FromQuery] string type)
        {
            var data = await _taskTemplateMdcsService.Set().Where(e => e.IsEnable && (e.Type != null ? e.Type.ToUpper().Contains(type.ToUpper()) : true)).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _taskTemplateMdcsService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _taskTemplateMdcsService.DisableAsync(keyValues);
            return Ok();
        }

    }

}
