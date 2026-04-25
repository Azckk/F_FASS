using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using DotNetCore.CAP;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Base;
using FASS.Data.Dtos.Data;
using FASS.Data.Services.Base.Interfaces;
using FASS.Data.Services.Data.Interfaces;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Services.DataExtend.Interfaces;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Models.Pc;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;
using FASS.Data.Consts.Data;

namespace FASS.Web.Api.Areas.Data.Controllers
{
    [Route("api/v1/Data/[controller]/[action]")]
    [Tags("数据管理-车辆管理")]
    public class CarController : BaseController
    {
        private readonly ILogger<CarController> _logger;
        private readonly ICarService _carService;
        private readonly ICarTypeService _carTypeService;
        private readonly ICapPublisher _capPublisher;
        private readonly ICarZoneService _carZoneService;
        private readonly IZoneService _zoneService;
        private readonly ITaskRecordService _taskRecordService;

        private readonly AppSettings _appSettings;

        public CarController(
            ILogger<CarController> logger,
            ICarService carService,
            ICarTypeService carTypeService,
            ICapPublisher capPublisher,
            ICarZoneService carZoneService,
            ITaskRecordService taskRecordService,
            IZoneService zoneService,
            AppSettings appSettings
        )
        {
            _logger = logger;
            _carService = carService;
            _carTypeService = carTypeService;
            _capPublisher = capPublisher;
            _carZoneService = carZoneService;
            _zoneService = zoneService;
            _appSettings = appSettings;
            _taskRecordService = taskRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carService.ToPageAsync(param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _carService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] CarDto carDto)
        {
            await _carService.AddOrUpdateAsync(keyValue, carDto);
            if (keyValue != null)
            {
                carDto.Id = keyValue;
            }
            await _capPublisher.PublishAsync("CarController.CarDto", carDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _carService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _carService.EnableAsync(keyValues);
            await _capPublisher.PublishAsync("CarController.Enable", keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _carService.DisableAsync(keyValues);
            await _capPublisher.PublishAsync("CarController.Disable", keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _carService
                .Set()
                .Where(e => e.IsEnable)
                .OrderBy(e => e.Code.Length)
                .ThenBy(e => e.Code)
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelectByCarTypeCode(
            [FromQuery] string carTypeCode
        )
        {
            var data = await _carService
                .Set()
                .Where(e => e.IsEnable)
                .Where(e => e.CarTypeCode == carTypeCode)
                .OrderBy(e => e.Code.Length)
                .ThenBy(e => e.Code)
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelectByTaskTemplateId(
            [FromQuery] string taskTemplateId
        )
        {
            var data = await _carService.GetListToSelectByTaskTemplateIdAsync(taskTemplateId);
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Select()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> SelectGetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carService.SelectGetPageAsync(param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Standby()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> StandbyGetPage(
            [FromQuery] string keyValue,
            [FromQuery] string pageParam
        )
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carService.StandbyGetPageAsync(keyValue, param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> StandbyAdd(
            [FromQuery] string keyValue,
            [FromBody] List<string> standbyIds
        )
        {
            await _carService.StandbyAddAsync(keyValue, standbyIds);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> StandbyDelete(
            [FromQuery] string keyValue,
            [FromBody] List<string> standbyIds
        )
        {
            await _carService.StandbyDeleteAsync(keyValue, standbyIds);
            return Ok();
        }

        //[HttpGet]
        //public IActionResult Charge()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> ChargeGetPage(
            [FromQuery] string keyValue,
            [FromQuery] string pageParam
        )
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carService.ChargeGetPageAsync(keyValue, param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> ChargeAdd(
            [FromQuery] string keyValue,
            [FromBody] List<string> chargeIds
        )
        {
            await _carService.ChargeAddAsync(keyValue, chargeIds);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> ChargeDelete(
            [FromQuery] string keyValue,
            [FromBody] List<string> chargeIds
        )
        {
            await _carService.ChargeDeleteAsync(keyValue, chargeIds);
            return Ok();
        }

        //[HttpGet]
        //public IActionResult Avoid()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> AvoidGetPage(
            [FromQuery] string keyValue,
            [FromQuery] string pageParam
        )
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carService.AvoidGetPageAsync(keyValue, param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AvoidAdd(
            [FromQuery] string keyValue,
            [FromBody] List<string> avoidIds
        )
        {
            await _carService.AvoidAddAsync(keyValue, avoidIds);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> AvoidDelete(
            [FromQuery] string keyValue,
            [FromBody] List<string> avoidIds
        )
        {
            await _carService.AvoidDeleteAsync(keyValue, avoidIds);
            return Ok();
        }

        //[HttpGet]
        //public IActionResult Node()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> NodeGetPage(
            [FromQuery] string keyValue,
            [FromQuery] string pageParam
        )
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carService.NodeGetPageAsync(keyValue, param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Edge()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> EdgeGetPage(
            [FromQuery] string keyValue,
            [FromQuery] string pageParam
        )
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carService.EdgeGetPageAsync(keyValue, param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> ActionInit([FromQuery] string carCode)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/reset/{carCode}"
                );
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 初始化失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]初始化失败 =>{ex}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActionRepair([FromQuery] string carCode)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/repair/{carCode}"
                );
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 现场维修失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]现场维修失败 =>{ex}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActionForceStop([FromQuery] string carCode)
        {
            if (string.IsNullOrEmpty(carCode))
            {
                return BadRequest("车辆代码不能为空。");
            }

            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"强制结束获取车辆失败，未找到车辆 [{carCode}]。");
                }

                // await _taskRecordService.GetObjectLock().WaitAsync();
                if (car.CarState == null)
                {
                    return BadRequest($"车辆 [{carCode}] 没有正在执行的任务。");
                }

                if (!car.CarState.TryJsonTo<CarState>(out var carState) || carState == null)
                {
                    return BadRequest($"车辆 [{carCode}] 没有正在执行的任务。");
                }

                var task = carState.Tasks.FirstOrDefault(e => e.State == "Running");
                TaskRecordDto dto = null!;
                if (task != null)
                {
                    dto = Guard.NotNull(await _taskRecordService.FirstOrDefaultAsync(e => e.CarId == car.Id && e.Id == task.Code && TaskRecordConst.State.Cancel.Contains(e.State)));
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>($"{_appSettings.Mdcs.SimpleUrl}/car/ForceStop/{carCode}");
                Console.WriteLine($"ActionForceStop--{resp.Code}--{resp.Message}--{resp.Data}");
                if (resp.Code == 200)
                {
                    await _capPublisher.PublishAsync("TaskInstanceController.Cancel", new TaskInstanceParam
                    {
                        TaskInstanceId = dto.Id,
                        CarId = dto.CarId
                    });

                    dto.State = TaskRecordConst.State.Canceled;
                    await _taskRecordService.UpdateTaskRecordWithInstanceAsync(new List<TaskRecordDto> { dto }, TaskInstanceConst.State.Canceling, WorkConst.State.Canceled);
                    return Ok();
                }

                return BadRequest($"车辆 [{carCode}] 现场强制结束失败)");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ActionForceStop: {ex}");
                return BadRequest($"车辆 [{carCode}] 现场强制结束失败，异常信息: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActionReStart([FromQuery] string carCode)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"重启小车获取车辆失败 [{carCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"http://{car.IpAddress}:9776/car/ReStart"
                );
                if (resp != null)
                {
                    if (resp.Code == 200)
                    {
                        return Ok();
                    }
                    else
                    {

                        return BadRequest($"车辆 [{carCode}] 重启失败 =>{resp.Message}");
                    }
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]重启失败 =>{ex}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActionBlown([FromQuery] string carCode)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/blown/{carCode}"
                );
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 返厂维修失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]返厂维修失败 =>{ex}");
            }
        }

        /// <summary>
        /// 暂停/恢复车辆
        /// </summary>
        /// <param name="Param"> 包含小车编号及启动恢复状态码</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ActionStopOrStart([FromQuery] string Param)
        {
            try
            {
                //var carChose = Param.JsonTo<CarChose>();
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == Param);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{Param}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/startOrPause/?CarCode={Param}"
                );
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"车辆 [{Param}] 启动或者暂停失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{Param}]启动或者暂停失败 =>{ex}");
            }
        }

        /// <summary>
        /// 停接任务
        /// </summary>
        /// <param name="Param"> 包含小车编号</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ActionStopReceiveTask([FromQuery] string carCode)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/stopReceiveTask/{carCode}"
                );
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 停接任务失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]停接任务失败 =>{ex}");
            }
        }

        /// <summary>
        /// 启动/停止
        /// </summary>
        /// <param name="carCode"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> startOrPause(
            [FromQuery] string carCode,
            [FromQuery] int ISRelease
        )
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/startOrPause/?CarCode={carCode}&ISRelease={ISRelease}"
                );
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 停接任务失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]停接任务失败 =>{ex}");
            }
        }

        /// <summary>
        /// 前往待命点
        /// </summary>
        /// <param name="carCode">小车编号 </param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ActionGoToStandby([FromQuery] string carCode)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }

                CarChose chose = new CarChose { CarCode = car.Code };
                var resp1 = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/goToStandby?CarCode={carCode}"
                );
                if (resp1.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 获取待命点失败");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]获取待命点失败 =>{ex}");
            }
        }

        /// <summary>
        /// 前往充电点
        /// </summary>
        /// <param name="carCode">小车编号 </param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ActionGoToCharge([FromQuery] string carCode)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }

                CarChose chose = new CarChose { CarCode = car.Code };
                var resp1 = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/goToCharge?CarCode={carCode}"
                );
                if (resp1.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 去充电失败");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]去充电点失败 =>{ex}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActionCarGoToSomePlace([FromQuery] string Param)
        {
            try
            {
                var carChose = Guard.NotNull(Param.JsonTo<CarChose>());
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carChose.CarCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carChose.CarCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/gosite?CarCode={carChose.CarCode}&id={carChose.Id}"
                );
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"车辆 [{carChose.CarCode}] 去某地失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆去某地失败 =>{ex}");
            }
        }

        /// <summary>
        /// 获取小车信息
        /// </summary>
        /// <param name="Param"> 小车编号</param>ActionGetCarInfo
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ActionGetCarInfo([FromQuery] string carCode)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"http://{_appSettings.Mdcs.SimpleUrl}/car/carInfo/{carCode}"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 获取小车信息失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]获取小车信息失败 =>{ex}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ZoneGetPage(
            [FromQuery] string keyValue,
            [FromQuery] string pageParam
        )
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            param.Where.ForEach(ex =>
            {
                if (ex.Field.Equals("name"))
                {
                    ex.Field = "remark";
                }
            });
            var page = await _carZoneService.ZoneGetPageAsync(keyValue, param);
            var zoneDtos = await _zoneService.Set().Where(e => e.IsEnable).ToListAsync();
            for (int i = 0; i < page.Data.Count; i++)
            {
                var zoneDto = zoneDtos.Where(e => e.Id == page.Data[i].ZoneId).FirstOrDefault();
                if (zoneDto != null)
                {
                    page.Data[i].ZoneCode = zoneDto.Code;
                    page.Data[i].ZoneName = zoneDto.Name;
                }
            }
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> ZoneAdd(
            [FromQuery] string keyValue,
            [FromBody] List<ZoneDto> zoneDtos
        )
        {
            await _carZoneService.ZoneAddAsync(keyValue, zoneDtos);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> ZoneDelete(
            [FromQuery] string keyValue,
            [FromBody] List<CarZoneDto> carZoneDtos
        )
        {
            await _carZoneService.ZoneDeleteAsync(keyValue, carZoneDtos);
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetSimpleCars()
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult<List<CarInfo>>>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/getAllCars"
                );
                if (resp.Code == 200)
                {
                    if (resp.Data is null)
                        return BadRequest($"simple中获取到车辆数据为空! {resp.ToJson()}");
                    List<CarDto> addCarDtos = new List<CarDto>();
                    List<CarDto> updateCarDtos = new List<CarDto>();
                    //获取所有已有车辆
                    var carDtos = await _carService.Set().ToListAsync();
                    var carTypeDtos = await _carTypeService.ToListAsync(e => e.IsEnable);
                    var defaultCarTypeId = Guard.NotNull(carTypeDtos.FirstOrDefault(e => e.Code == "Car")?.Id);

                    foreach (var info in resp.Data)
                    {
                        var dto = carDtos.Where(e => e.Code == info.Code).FirstOrDefault();
                        if (dto is null)
                        {
                            var car = new CarDto
                            {
                                Code = info.Code,
                                Name = info.Name,
                                IpAddress = info.Address,
                                Port = 8222,
                                Type = "Fairyland.Pc",
                                CarTypeId = carTypeDtos.Where(e => e.Code == info.CarType).FirstOrDefault()?.Id ?? defaultCarTypeId,
                                Length = info.Length == 0 ? 900 : info.Length,
                                Width = info.Width == 0 ? 455 : info.Width,
                                Battery = info.Battery,
                                ControlType = CarConst.ControlType.Automatic,
                                AvoidType = CarConst.AvoidType.CanNotAvoid,
                                MinBattery = 30,
                                MaxBattery = 80
                            };
                            if(info.CurrNodeCode != "0")
                                car.CurrNodeId = info.CurrNodeCode;
                            addCarDtos.Add(car);
                        }
                        else
                        {
                            //存在车辆信息，更新车辆名称，车型
                            dto.Name = info.Name;
                            dto.CarTypeId = carTypeDtos.Where(e => e.Code == info.CarType).FirstOrDefault()?.Id ?? defaultCarTypeId;
                            updateCarDtos.Add(dto);
                        }
                    }
                    if (addCarDtos.Count > 0)
                    {
                        await _carService.AddAsync(addCarDtos);
                    }
                    //if (updateCarDtos.Count > 0) {
                    //    await _carService.UpdateAsync(addCarDtos);
                    //}
                    return Ok($"新增{addCarDtos.Count}台车辆");
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"同步车辆信息失败：{ex.Message}");
            }
        }

    }
}
