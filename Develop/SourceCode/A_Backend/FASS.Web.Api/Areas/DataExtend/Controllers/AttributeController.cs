using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Services.DataExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.DataExtend.Controllers
{
    [Route("api/v1/Data/[controller]/[action]")]
    [Tags("数据管理-属性配置")]
    public class AttributeController : BaseController
    {
        private readonly ILogger<AttributeController> _logger;
        private readonly IAttributeService _attributeService;

        public AttributeController(
            ILogger<AttributeController> logger,
            IAttributeService attributeService)
        {
            _logger = logger;
            _attributeService = attributeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _attributeService.ToPageAsync(param);
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
            var data = await _attributeService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] AttributeDto attributeDto)
        {
            await _attributeService.AddOrUpdateAsync(keyValue, attributeDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _attributeService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _attributeService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _attributeService.DisableAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelectByKind([FromQuery] string kind)
        {
            var data = await _attributeService.Set().Where(e => e.IsEnable && e.Kind == kind.ToLower()).OrderBy(e => e.SortNumber).ToListAsync();
            return Ok(data);
        }

    }
}
