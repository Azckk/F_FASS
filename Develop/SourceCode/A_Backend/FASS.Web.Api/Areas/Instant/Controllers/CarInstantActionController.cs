using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Consts.Instant;
using FASS.Data.Dtos.Instant;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Instant.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Instant.Controllers
{
    [Route("api/v1/Instant/[controller]/[action]")]
    [Tags("流程管理-即时动作-车辆动作")]
    public class CarInstantActionController : BaseController
    {
        private readonly ILogger<CarInstantActionController> _logger;
        private readonly ICarInstantActionService _carInstantActionService;
        private readonly ICarService _carService;

        public CarInstantActionController(
            ILogger<CarInstantActionController> logger,
            ICarInstantActionService carInstantActionService,
            ICarService carService)
        {
            _logger = logger;
            _carInstantActionService = carInstantActionService;
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carInstantActionService.ToPageAsync(param);
            var carDtos = await _carService.Set().ToListAsync();
            for (int i = 0; i < page.Data.Count; i++)
            {
                page.Data[i].Remark = carDtos.Where(e => e.Code == page.Data[i].CarCode).FirstOrDefault()?.Name;
            }
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
            var data = await _carInstantActionService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] CarInstantActionDto carInstantActionDto)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                carInstantActionDto.State = CarInstantActionConst.State.Created;
            }
            else
            {
                var carInstantAction = _carInstantActionService.FirstOrDefault(keyValue);
                if (carInstantAction == null)
                {
                    return BadRequest("即时动作不存在，无法进行操作");
                }
                if (!CarInstantActionConst.State.Update.Contains(carInstantAction.State))
                {
                    return BadRequest($"即时动作：[ {carInstantActionDto.ActionType} ] 已进入处理队列，无法操作");
                }
                carInstantActionDto.State = carInstantAction.State;
            }
            await _carInstantActionService.AddOrUpdateAsync(keyValue, carInstantActionDto);
            var result = await _carInstantActionService.AnyAsync(e => e.Id == carInstantActionDto.Id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            var dtos = await _carInstantActionService.ToListAsync(e => keyValues.Contains(e.Id) && CarInstantActionConst.State.Delete.Contains(e.State));
            var data = await _carInstantActionService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> ForceDelete([FromBody] List<string> keyValues)
        {
            var data = await _carInstantActionService.DeleteAsync(keyValues);
            return Ok($"操作成功");
        }

        [HttpPut]
        public async Task<IActionResult> Release([FromBody] List<string> keyValues)
        {
            var dtos = await _carInstantActionService.ToListAsync(e => keyValues.Contains(e.Id) && CarInstantActionConst.State.Release.Contains(e.State));
            foreach (var dto in dtos)
            {
                dto.State = CarInstantActionConst.State.Released;
            }
            await _carInstantActionService.UpdateAsync(dtos);
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpPut]
        public async Task<IActionResult> Cancel([FromBody] List<string> keyValues)
        {
            var dtos = await _carInstantActionService.ToListAsync(e => keyValues.Contains(e.Id) && CarInstantActionConst.State.Cancel.Contains(e.State));
            foreach (var dto in dtos)
            {
                dto.State = CarInstantActionConst.State.Canceling;
            }
            await _carInstantActionService.UpdateAsync(dtos);
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM3()
        {
            var dtos = await _carInstantActionService.ToListAsync(e => e.CreateAt < DateTime.Now.AddMonths(-3) && CarInstantActionConst.State.Delete.Contains(e.State));
            var data = await _carInstantActionService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM1()
        {
            var dtos = await _carInstantActionService.ToListAsync(e => e.CreateAt < DateTime.Now.AddMonths(-1) && CarInstantActionConst.State.Delete.Contains(e.State));
            var data = await _carInstantActionService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteW1()
        {
            var dtos = await _carInstantActionService.ToListAsync(e => e.CreateAt < DateTime.Now.AddDays(-7) && CarInstantActionConst.State.Delete.Contains(e.State));
            var data = await _carInstantActionService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteD1()
        {
            var dtos = await _carInstantActionService.ToListAsync(e => e.CreateAt < DateTime.Now.AddDays(-1) && CarInstantActionConst.State.Delete.Contains(e.State));
            var data = await _carInstantActionService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var dtos = await _carInstantActionService.ToListAsync(e => CarInstantActionConst.State.Delete.Contains(e.State));
            var data = await _carInstantActionService.DeleteAsync(dtos.Select(e => e.Id));
            return Ok($"操作成功 [{dtos.Count}]");
        }
    }
}