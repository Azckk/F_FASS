using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Instant;
using FASS.Data.Services.Instant.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Instant.Controllers
{
    [Route("api/v1/Instant/[controller]/[action]")]
    [Tags("流程管理-即时动作-车辆动作-参数")]
    public class CarInstantParameterController : BaseController
    {
        private readonly ILogger<CarInstantParameterController> _logger;
        private readonly ICarInstantParameterService _carInstantParameterService;

        public CarInstantParameterController(
            ILogger<CarInstantParameterController> logger,
            ICarInstantParameterService carInstantParameterService)
        {
            _logger = logger;
            _carInstantParameterService = carInstantParameterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string carInstantActionId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carInstantParameterService.ToPageAsync(carInstantActionId, param);
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
            var data = await _carInstantParameterService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] CarInstantParameterDto carInstantParameterDto)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                if (string.IsNullOrWhiteSpace(carInstantParameterDto.Key))
                {
                    return BadRequest($"键 [ {carInstantParameterDto.Key} ] 空值");
                }
                if (await _carInstantParameterService.AnyAsync(e => e.ActionId == carInstantParameterDto.ActionId && e.Key.ToUpper() == carInstantParameterDto.Key.ToUpper()))
                {
                    return BadRequest($"键 [ {carInstantParameterDto.Key} ] 已存在");
                }
            }
            await _carInstantParameterService.AddOrUpdateAsync(keyValue, carInstantParameterDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _carInstantParameterService.DeleteAsync(keyValues);
            return Ok();
        }
    }
}