using Common.NETCore;
using Common.NETCore.Extensions;
using Common.Service.Pager.Models;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{
    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-预定物料信息")]
    public class PreMaterialController : BaseController
    {
        private readonly ILogger<PreMaterialController> _logger;
        private readonly IPreMaterialService _preMaterialService;

        public PreMaterialController(
            ILogger<PreMaterialController> logger,
            IPreMaterialService preMaterialService)
        {
            _logger = logger;
            _preMaterialService = preMaterialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _preMaterialService.ToPageAsync(param);
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
            var data = await _preMaterialService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _preMaterialService.DeleteAsync(keyValues);
            return Ok();
        }
    }

}
