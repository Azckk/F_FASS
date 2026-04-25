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
    [Tags("移动管理-储位数据")]
    public class WarehouseDataStorageController : BaseController
    {
        private readonly ILogger<WarehouseDataStorageController> _logger;
        private readonly IStorageService _storageService;

        public WarehouseDataStorageController(
            ILogger<WarehouseDataStorageController> logger,
            IStorageService storageService)
        {
            _logger = logger;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _storageService.ToPageAsync(param);
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
            var data = await _storageService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] string keyValue, [FromBody] StorageDto storageDto)
        {
            await _storageService.Repository
                .ExecuteUpdateAsync(e => e.Id == keyValue, s => s
                .SetProperty(b => b.State, storageDto.State)
                .SetProperty(b => b.Barcode, storageDto.Barcode)
                .SetProperty(b => b.IsLock, storageDto.IsLock));
            return Ok();
        }
    }
}