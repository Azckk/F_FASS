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
    [Tags("仓储管理-储位容器历史")]
    public class StorageContainerHistoryController : BaseController
    {
        private readonly ILogger<StorageContainerHistoryController> _logger;
        private readonly IStorageContainerHistoryService _storageContainerHistoryService;

        public StorageContainerHistoryController(
            ILogger<StorageContainerHistoryController> logger,
            IStorageContainerHistoryService storageContainerHistoryService)
        {
            _logger = logger;
            _storageContainerHistoryService = storageContainerHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _storageContainerHistoryService.ToPageAsync(param);
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
            var data = await _storageContainerHistoryService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM3()
        {
            await _storageContainerHistoryService.DeleteM3Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM1()
        {
            await _storageContainerHistoryService.DeleteM1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteW1()
        {
            await _storageContainerHistoryService.DeleteW1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteD1()
        {
            await _storageContainerHistoryService.DeleteD1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAll()
        {
            await _storageContainerHistoryService.DeleteAllAsync();
            return Ok();
        }

    }
}
