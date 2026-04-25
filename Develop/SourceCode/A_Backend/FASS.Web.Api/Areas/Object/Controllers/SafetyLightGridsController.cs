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
    [Tags("数据管理-安全光栅")]
    public class SafetyLightGridsController : BaseController
    {
        private readonly ILogger<SafetyLightGridsController> _logger;

        private readonly ISafetyLightGridsService _safetyLightGridsService;

        private readonly AppSettings _appSettings;

        public SafetyLightGridsController(
            ILogger<SafetyLightGridsController> logger,ISafetyLightGridsService safetyLightGridsService,
           AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
            _safetyLightGridsService=safetyLightGridsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _safetyLightGridsService.ToPageAsync(param);

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
            var page = await _safetyLightGridsService.ToPageAsync(param);
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
            var data = await _safetyLightGridsService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] SafetyLightGridDto safetyLightGridDto)
        {
            var result = await _safetyLightGridsService.AddOrUpdateAsync(keyValue, safetyLightGridDto);
      
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _safetyLightGridsService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
           
            var data = await _safetyLightGridsService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

     

        [HttpGet]
        public async Task<IActionResult> ItemsGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _safetyLightGridsService.ItemsGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }


        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _safetyLightGridsService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _safetyLightGridsService.DisableAsync(keyValues);
            return Ok();
        }

    }
}
