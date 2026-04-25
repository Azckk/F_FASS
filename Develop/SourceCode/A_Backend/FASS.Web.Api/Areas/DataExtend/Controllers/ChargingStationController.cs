using Common.NETCore;
using Common.NETCore.Extensions;
using Common.Service.Pager.Models;
using FASS.Data.Models.Data;
using FASS.Data.Services.Data.Interfaces;
using FASS.Extend.Charge.Pcb;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Services.DataExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FASS.Web.Api.Areas.DataExtend.Controllers
{

    [Route("api/v1/Data/[controller]/[action]")]
    [Tags("数据管理-充电桩管理")]
    public class ChargingStationController : BaseController
    {
        private readonly ILogger<ChargingStationController> _logger;
        private readonly IChargingStationService _chargingStationService;
        private readonly IChargeService _chargeService;

        public ChargingStationController(
            ILogger<ChargingStationController> logger,
            IChargingStationService chargingStationService,
            IChargeService chargeService)
        {
            _logger = logger;
            _chargingStationService = chargingStationService;
            _chargeService = chargeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _chargingStationService.ToPageAsync(param);
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
            var data = await _chargingStationService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] ChargingStationDto chargingStationDto)
        {
            if (!await _chargeService.AnyAsync(e => e.Id == chargingStationDto.ChargeId))
            {
                return BadRequest($"充电点不存在，充电点编号[{chargingStationDto.ChargeCode}]");
            }
            if (await _chargingStationService.AnyAsync(e => !string.IsNullOrEmpty(keyValue) ? (e.ChargeId == chargingStationDto.ChargeId && e.Id != keyValue) : (e.ChargeId == chargingStationDto.ChargeId)))
            {
                return BadRequest($"充电点已被绑定，充电点编号 [{chargingStationDto.ChargeId}]");
            }
            await _chargingStationService.AddOrUpdateAsync(keyValue, chargingStationDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _chargingStationService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _chargingStationService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }


        //[HttpGet]
        //public async Task<IActionResult> testChargeUdp()
        //{
        //    var data = await _chargingStationService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
        //    data.Add(new ChargingStationDto()
        //    {
        //        Code = "test",
        //        Ip = "127.0.0.1",
        //        Port = 2000,
        //        ChargeId = "10",
        //        ChargeCode = "100",
        //        Mode = "Side",
        //        Protocol = "tcp",

        //    });
        //    List<Car> cars = new List<Car>();
        //    var command = new Command(data, cars);
        //    return Ok(data);
        //}
    }

}
