using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Dtos.RecordExtend;
using FASS.Service.Models.Report;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.RecordExtend.Interfaces;
using FASS.Service.Services.Report.Interfaces;
using FASS.Web.Api.Areas.Report.Models;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Report.Controllers
{
    [Route("api/v1/Report/[controller]/[action]")]
    [Tags("报表统计-效率报表")]
    public class EfficiencyController : BaseController
    {
        private readonly ILogger<EfficiencyController> _logger;
        private readonly IDataService _dataService;
        private readonly Dictionary<string, Type> _typeWhere;
        private readonly ITaskRecordService _taskRecordService;

        public readonly IDisChargeConsumeService _disChargeConsumeService;
        public readonly IChargeConsumeService _chargeConsumeService;

        public EfficiencyController(
            ILogger<EfficiencyController> logger,
            ITaskRecordService taskRecordService,
            IDataService dataService,
            IDisChargeConsumeService disChargeConsumeService,
            IChargeConsumeService chargeConsumeService
        )
        {
            _logger = logger;
            _dataService = dataService;
            _taskRecordService = taskRecordService;
            _disChargeConsumeService = disChargeConsumeService;
            _chargeConsumeService = chargeConsumeService;
            _typeWhere = new Dictionary<string, Type>()
            {
                { "createAtStart", typeof(DateTime) },
                { "createAtEnd", typeof(DateTime) },
                { "carId", typeof(string) }
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var dictWhere = param.Where.ToDictionary(
                e => e.Field,
                e => e.Value.ConvertTo(_typeWhere[e.Field])
            );
            var data = await _dataService.GetDataAsync(dictWhere);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _dataService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlarmReport([FromQuery] string Param)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            /* int day = int.Parse(Param);
             if (day == 0)
             {
                 startDate = DateTime.Now.Date; // 获取今天的日期
             }
             else
             {
                 startDate = DateTime.Now.AddDays(-day);
             }*/
            RequestParam request = Guard.NotNull(Param.JsonTo<RequestParam>());
            Dictionary<string, DateTime> Where = new Dictionary<string, DateTime>();
            Where.Add("createAtStart", DateTime.ParseExact(
                            (request.CreateAtStart),
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture
                        ));
            Where.Add("createAtEnd", DateTime.ParseExact(
                            (request.CreateAtEnd),
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture
                        ));
            var data = await _dataService.GetAlarmAsync(Where);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskReport([FromQuery] string Param)
        {
            RequestParam request = Guard.NotNull(Param.JsonTo<RequestParam>());

            Dictionary<string, object> Where = new Dictionary<string, object>();
            //DateTime.ParseExact((request.createAtStart), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
            Where.Add("createAtStart", request.CreateAtStart.ToDateTime().ToString("yyyy-MM-dd"));
            Where.Add("createAtEnd", request.CreateAtEnd.ToDateTime().ToString("yyyy-MM-dd"));

            var data = await _dataService.GetTaskAsync(Where);
            var result = new
            {
                taskDayList = data.Item1.ToExpandoObjects(),
                taskTotal = data.Item2.Rows.Count > 0 ? data.Item2.Rows[0].ToExpandoObject() :
                new
                {
                    success = 0,
                    failure = 0,
                    total = 0
                }
            };
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetChargeConsumeReport([FromQuery] string Param)
        {
            RequestParam request = Guard.NotNull(Param.JsonTo<RequestParam>());

            Dictionary<string, object> Where = new Dictionary<string, object>();
            //DateTime.ParseExact((request.createAtStart), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
            Where.Add("createAtStart", request.CreateAtStart);
            Where.Add("createAtEnd", request.CreateAtEnd);
            var data = await _dataService.GetChargeConsumeAsync(Where);
            List<Consume> chargeconsumes = new List<Consume>();
            List<Consume> dischargeconsumes = new List<Consume>();
            for (int i = 0; i < data.Item1.Rows.Count; i++)
            {
                var chargeTime = data.Item1.Rows[i]["chargetime"].ToString();
                if (chargeTime != null)
                {
                    chargeconsumes.Add(new Consume
                    {
                        Dn = data.Item1.Rows[i]["dn"].ToString(),
                        ChargeTime = DateTime.Parse(chargeTime).ToString("yyyy-MM-dd")
                    });
                }
            }
            for (int i = 0; i < data.Item2.Rows.Count; i++)
            {
                var chargeTime = data.Item2.Rows[i]["chargetime"].ToString();
                if (chargeTime != null)
                {
                    dischargeconsumes.Add(new Consume
                    {
                        Dn = data.Item2.Rows[i]["dn"].ToString(),
                        ChargeTime = DateTime.Parse(chargeTime).ToString("yyyy-MM-dd")
                    });
                }
            }
            var result = new
            {
                ChargeList = chargeconsumes,
                DisChargeList = dischargeconsumes
            };
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCarChargeConsumeReport([FromQuery] string Param)
        {
            /*RequestParam request = Param.JsonTo<RequestParam>();

            Dictionary<string, object> Where = new Dictionary<string, object>();
            //DateTime.ParseExact((request.createAtStart), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
            Where.Add("createAtStart", request.createAtStart);
            Where.Add("createAtEnd", request.createAtEnd);
            var data = await _dataService.GetChargeConsumeAsync(Where);*/

            RequestParam request = Guard.NotNull(Param.JsonTo<RequestParam>());

            List<ChargeConsumeDto> data = await _chargeConsumeService
                .Set()
                .Where(e =>
                    e.CreateAt
                        >= DateTime.ParseExact(
                            (request.CreateAtStart),
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture
                        )
                    && e.CreateAt
                        <= DateTime.ParseExact(
                            (request.CreateAtEnd),
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture
                        )
                    && e.CarCode == request.CarCode
                )
                .ToListAsync();

            List<DisChargeConsumeDto> data2 = await _disChargeConsumeService
                .Set()
                .Where(e =>
                    e.CreateAt
                        >= DateTime.ParseExact(
                            (request.CreateAtStart),
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture
                        )
                    && e.CreateAt
                        <= DateTime.ParseExact(
                            (request.CreateAtEnd),
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture
                        )
                    && e.CarCode == request.CarCode
                )
                .ToListAsync();
            return Ok((data, data2));
        }


        [HttpGet]
        public async Task<IActionResult> test([FromQuery] string Param)
        {


            RequestParam request = Guard.NotNull(Param.JsonTo<RequestParam>());

            await _dataService.insertAlarm();

            return Ok();
        }



    }
}
