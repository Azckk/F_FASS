using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.FlowExtend.Controllers
{

    [Route("api/v1/Flow/[controller]/[action]")]
    [Tags("流程管理-物流动线(Mdcs)")]
    public class LogisticsRouteController : BaseController
    {
        private readonly ILogger<LogisticsRouteController> _logger;
        private readonly ILogisticsRouteService _logisticsRouteService;

        public LogisticsRouteController(
            ILogger<LogisticsRouteController> logger,
            ILogisticsRouteService logisticsRouteService)
        {
            _logger = logger;
            _logisticsRouteService = logisticsRouteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _logisticsRouteService.ToPageAsync(param);
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
            var data = await _logisticsRouteService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] LogisticsRouteDto logisticsRouteDto)
        {
            await _logisticsRouteService.AddOrUpdateAsync(keyValue, logisticsRouteDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _logisticsRouteService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _logisticsRouteService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }


    }


}
