using Common.NETCore.Extensions;
using FASS.Scheduler.Attributes;
using FASS.Scheduler.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Areas.Signal.Controllers
{
    [AllowAnonymous]
    [TypeFilter(typeof(AuthorizeActionIgonreAttribute))]
    [TypeFilter(typeof(ActionLogIgonreAttribute))]
    [Tags("安全信号对接")]
    public class SecuritySignalController : BaseController
    {
        private readonly ILogger<SecuritySignalController> _logger;
        private readonly IDistributedCache _distributedCache;

        public SecuritySignalController(
            ILogger<SecuritySignalController> logger,
            IDistributedCache distributedCache
        )
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<IActionResult> GetSecuritySignal([FromBody] string signalInfo)
        {
            try
            {
                Console.WriteLine("接收到的参数： " + signalInfo.ToJson());
                await _distributedCache.SetStringAsync("signa", signalInfo);
                if (!string.IsNullOrWhiteSpace(signalInfo))
                {
                    var Allsignal = signalInfo.JsonTo<Dictionary<string, Dictionary<string, bool>>>();
                    //Dictionary<string, Dictionary<string, bool>> Allsignal = signalInfo.JsonTo<Dictionary<string, Dictionary<string, bool>>>();
                    if (Allsignal != null)
                    {
                        foreach (var outerKey in Allsignal.Keys)
                        {
                            var innerDict = Allsignal[outerKey];
                            await _distributedCache.SetStringAsync(outerKey, innerDict.ToJson());
                        }
                    }
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            // 在这里使用param参数进行操作
            #region
            /*    using (var connection = ConnectionMultiplexer.Connect(configurationOptions))
                {
                    // 获取数据库
                    var db = connection.GetDatabase();

                    // 设置键值对
                    db.StringSet("signal",signalInfo);

                    // 获取键的值
                    var myValue = db.StringGet("signal");
                    Console.WriteLine($"The value of signal is: {myValue}");
                }*/
            #endregion
        }
        #region 获取安全信号信息
        /*  [HttpGet]
          public async Task<IActionResult> SecuritySignal([FromQuery] string storageName)
          {
              // 在这里使用param参数进行操作

              try
              {
                  #region
                  */
        /* using (var connection = ConnectionMultiplexer.Connect(configurationOptions))
                           {
                               // 获取数据库
                               var db = connection.GetDatabase();
                               // 获取键的值
                               var myValue = db.StringGet("signal");
        
                               Console.WriteLine($"The value of signal is: {myValue}");
                               return Ok(myValue);
                           }*/
        /*
                          #endregion
                          // 获取键的值
                          var myValue = "";
                          Console.WriteLine($"The value of signal is: {storageName}");
                          if (storageName == null || storageName == "")
                          {
                              myValue = await _distributedCache.GetStringAsync("signa");
        
                              Console.WriteLine($"The value of signa is: {myValue}");
                          }
                          else
                          {
                              myValue = await _distributedCache.GetStringAsync(storageName);
        
                              Console.WriteLine($"The value of signa is: {myValue}");
                          }
                          if (myValue == null)
                          {
                              myValue = "没有缓存";
                          }
        
                          return Ok(myValue);
                      }
                      catch (Exception e)
                      {
                          return BadRequest(e.Message);
                      }
                  }*/
        #endregion
    }
}
