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
    [Tags("数据管理-自动门")]
    public class AutoDoorController : BaseController
    {
        private readonly ILogger<AutoDoorController> _logger;
        private readonly IAutoDoorService _autoDoorService;

        public AutoDoorController(ILogger<AutoDoorController> logger, IAutoDoorService autoDoorService)
        {
            _logger = logger;
            this._autoDoorService = autoDoorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _autoDoorService.ToPageAsync(param);
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
            var data = await _autoDoorService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] AutoDoorDto doorDto)
        {
            await _autoDoorService.AddOrUpdateAsync(keyValue, doorDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _autoDoorService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _autoDoorService.Set().Where(e => e.IsEnable).OrderBy(e => e.CreateAt).ThenBy(e => e.CreateAt).ToListAsync();

            return Ok(data);
        }
    }
}
