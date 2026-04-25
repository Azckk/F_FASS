using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Warehouse.Controllers
{
    [Route("api/v1/Warehouse/[controller]/[action]")]
    [Tags("仓储管理-容器物料历史")]
    public class ContainerMaterialHistoryController : BaseController
    {
        private readonly ILogger<ContainerMaterialHistoryController> _logger;
        private readonly IContainerMaterialHistoryService _containerMaterialHistoryService;

        public ContainerMaterialHistoryController(
            ILogger<ContainerMaterialHistoryController> logger,
            IContainerMaterialHistoryService containerMaterialHistoryService)
        {
            _logger = logger;
            _containerMaterialHistoryService = containerMaterialHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _containerMaterialHistoryService.ToPageAsync(param);
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
            var data = await _containerMaterialHistoryService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM3()
        {
            await _containerMaterialHistoryService.DeleteM3Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM1()
        {
            await _containerMaterialHistoryService.DeleteM1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteW1()
        {
            await _containerMaterialHistoryService.DeleteW1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteD1()
        {
            await _containerMaterialHistoryService.DeleteD1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAll()
        {
            await _containerMaterialHistoryService.DeleteAllAsync();
            return Ok();
        }

    }
}
