using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{
    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-物料数据")]
    public class WarehouseDataMaterialController : BaseController
    {
        private readonly ILogger<WarehouseDataMaterialController> _logger;
        private readonly IMaterialService _materialService;

        public WarehouseDataMaterialController(
            ILogger<WarehouseDataMaterialController> logger,
            IMaterialService materialService)
        {
            _logger = logger;
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _materialService.ToPageAsync(param);
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
            var data = await _materialService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] string keyValue, [FromBody] MaterialDto materialDto)
        {
            await _materialService.Repository
                .ExecuteUpdateAsync(e => e.Id == keyValue, s => s
                .SetProperty(b => b.State, materialDto.State)
                .SetProperty(b => b.Barcode, materialDto.Barcode)
                .SetProperty(b => b.IsLock, materialDto.IsLock));
            return Ok();
        }
    }
}