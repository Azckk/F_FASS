using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Dtos.Object;
using FASS.Service.Services.Object.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Object.Controllers
{
    [Route("api/v1/Object/[controller]/[action]")]
    [Tags("数据管理-安全光栅地址位")]
    public class SafetyLightGridsItemController : BaseController
    {
        private readonly ILogger<SafetyLightGridsItemController> _logger;

        private readonly ISafetyLightGridsItemService _safetyLightGridsItemService;

        private readonly AppSettings _appSettings;

        public SafetyLightGridsItemController(
            ILogger<SafetyLightGridsItemController> logger,ISafetyLightGridsItemService safetyLightGridsItemService,
           AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
            _safetyLightGridsItemService = safetyLightGridsItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _safetyLightGridsItemService.ToPageAsync(param);

            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetPageSelect([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _safetyLightGridsItemService.ToPageAsync(param);
            var fileterDate = page.Data.Where(e => e.IsEnable).ToList();
            var data = new
            {
                rows = fileterDate,
                total = page.TotalCount
            };
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _safetyLightGridsItemService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] SafetyLightGridsItemDto safetyLightGridItemDto)
        {
            var result = await _safetyLightGridsItemService.AddOrUpdateAsync(keyValue, safetyLightGridItemDto);
      
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _safetyLightGridsItemService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
           
            var data = await _safetyLightGridsItemService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

     

     

    }
}
