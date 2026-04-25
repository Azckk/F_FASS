using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Dtos.Object;
using FASS.Service.Services.Object.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Object.Controllers
{
    [Route("api/v1/Object/[controller]/[action]")]
    [Tags("数据管理-第三方系统")]
    public class ThirdpartySystemController : BaseController
    {
        private readonly ILogger<ThirdpartySystemController> _logger;
        private readonly IThirdpartySystemService _thirdpartySystem;

        public ThirdpartySystemController(
            ILogger<ThirdpartySystemController> logger,
            IThirdpartySystemService thirdpartySystem
        )
        {
            _logger = logger;
            _thirdpartySystem = thirdpartySystem;
        }



        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _thirdpartySystem.ToPageAsync(param);
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
            var data = await _thirdpartySystem.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] ThirdpartySystemDto systemDto)
        {
            await _thirdpartySystem.AddOrUpdateAsync(keyValue, systemDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _thirdpartySystem.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _thirdpartySystem.Set().Where(e => e.IsEnable).OrderBy(e => e.CreateAt).ThenBy(e => e.CreateAt).ToListAsync();

            return Ok(data);
        }



    }
}
