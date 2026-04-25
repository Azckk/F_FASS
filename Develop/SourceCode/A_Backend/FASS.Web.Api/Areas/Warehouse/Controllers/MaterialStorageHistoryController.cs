using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Services.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Warehouse.Controllers
{
    [Route("api/v1/Warehouse/[controller]/[action]")]
    [Tags("仓储管理-物料储位历史")]
    public class MaterialStorageHistoryController : BaseController
    {
        private readonly ILogger<MaterialStorageHistoryController> _logger;
        private readonly IMaterialStorageHistoryService _materialStorageHistoryService;

        public MaterialStorageHistoryController(
            ILogger<MaterialStorageHistoryController> logger,
            IMaterialStorageHistoryService materialStorageHistoryService)
        {
            _logger = logger;
            _materialStorageHistoryService = materialStorageHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _materialStorageHistoryService.ToPageAsync(param);
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
            var data = await _materialStorageHistoryService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM3()
        {
            await _materialStorageHistoryService.DeleteM3Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM1()
        {
            await _materialStorageHistoryService.DeleteM1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteW1()
        {
            await _materialStorageHistoryService.DeleteW1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteD1()
        {
            await _materialStorageHistoryService.DeleteD1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAll()
        {
            await _materialStorageHistoryService.DeleteAllAsync();
            return Ok();
        }

    }
}
