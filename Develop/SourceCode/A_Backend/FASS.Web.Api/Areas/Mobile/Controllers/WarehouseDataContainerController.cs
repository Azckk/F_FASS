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
    [Tags("移动管理-容器数据")]
    public class WarehouseDataContainerController : BaseController
    {
        private readonly ILogger<WarehouseDataContainerController> _logger;
        private readonly IContainerService _containerService;

        public WarehouseDataContainerController(
            ILogger<WarehouseDataContainerController> logger,
            IContainerService containerService)
        {
            _logger = logger;
            _containerService = containerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _containerService.ToPageAsync(param);
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
            var data = await _containerService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] string keyValue, [FromBody] ContainerDto containerDto)
        {
            await _containerService.Repository
                .ExecuteUpdateAsync(e => e.Id == keyValue, s => s
                .SetProperty(b => b.State, containerDto.State)
                .SetProperty(b => b.Barcode, containerDto.Barcode)
                .SetProperty(b => b.IsLock, containerDto.IsLock));
            return Ok();
        }
    }
}