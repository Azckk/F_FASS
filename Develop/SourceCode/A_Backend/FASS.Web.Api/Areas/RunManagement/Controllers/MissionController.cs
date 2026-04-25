using Common.NETCore;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Models.Pc;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FASS.Web.Api.Areas.RunManagement.Controllers
{
    [Route("api/v1/RunManagement/[controller]/[action]")]
    [Tags("运行管理-进程管理")]
    public class MissionController : BaseController
    {
        private readonly ILogger<MissionController> _logger;
        private readonly AppSettings _appSettings;

        public MissionController(
            ILogger<MissionController> logger,
            AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult GetPage()
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/mission_reflection/get_mission_list"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"获取mission list失败：{ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetMissionFields([FromQuery] int missionId)
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/mission_reflection/get_fields/{missionId}"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"获取mission fields失败：{ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetMissionStatus([FromQuery] int missionId)
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/mission_reflection/get_status/{missionId}"
                );
                if (resp.Code == 200)
                {
                    var list = new List<Parameter>();
                    if (resp.Data is not null)
                    {
                        var root = Guard.NotNull(resp.Data.ToString()).JsonParse().RootElement;
                        foreach (JsonProperty pi in root.EnumerateObject())
                        {
                            list.Add(new Parameter { Key = pi.Name, Value = pi.Value.ToString() });
                        }
                    }
                    return Ok(list);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"获取mission status失败：{ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetMissionMethods([FromQuery] int missionId)
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/mission_reflection/get_mission/{missionId}"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"获取mission method失败：{ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult SetMissionFields([FromQuery] int missionId, [FromQuery] string key, [FromQuery] string value)
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/mission_reflection/set_field/{missionId}/{key}/{value}"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"设置mission fields失败 missionId:[{missionId}],key:[{key}],value:{value} {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult DeleteMissionField([FromQuery] int missionId, [FromQuery] string key)
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/mission_reflection/delete_field/{missionId}/{key}"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"删除 mission field失败 missionId:[{missionId}],key:[{key}] {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ExecuteMissionMethod([FromQuery] int missionId, [FromQuery] string method, [FromQuery] string? param)
        {
            try
            {
                string paramString = string.Empty;
                var paramters = param?.Replace("undefined","")?.JsonTo<List<Parameter>>();
                if (paramters is not null)
                {
                    foreach (var item in paramters)
                    {
                        paramString += $"&{item.Key}={item.Value}";
                    }
                    paramString = $"?{paramString.TrimStart("&")}";
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/mission_reflection/execute/{missionId}/{method}{paramString}"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"执行 mission method失败 missionId:[{missionId}],method:[{method}],param:{param} {ex.Message}");
            }
        }

    }
}
