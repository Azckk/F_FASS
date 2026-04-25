using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Areas.Signal.Controllers
{
    [Route("api/v1/signal/[controller]/[action]")]
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



        [HttpGet]
        public async Task<IActionResult> SecuritySignal([FromQuery] string storageName, [FromQuery] string storageCode)
        {
            // 在这里使用param参数进行操作

            try
            {
                var keyName = "";
                switch (storageCode)
                {
                    case "1":
                        keyName = "One";
                        break;
                    case "2":
                        keyName = "Two";
                        break;
                    case "3":
                        keyName = "Three";
                        break;
                    case "4":
                        keyName = "Four";
                        break;
                    case "5":
                        keyName = "Five";
                        break;
                    case "6":
                        keyName = "Six";
                        break;
                    case "7":
                        keyName = "Seven";
                        break;
                    case "8":
                        keyName = "Eight";
                        break;
                    case "9":
                        keyName = "Nine";
                        break;
                    case "10":
                        keyName = "Ten";
                        break;
                    case "11":
                        keyName = "Eleven";
                        break;
                    case "12":
                        keyName = "Twelve";
                        break;
                    case "13":
                        keyName = "Thirteen";
                        break;
                    case "14":
                        keyName = "Fourteen";
                        break;
                    case "15":
                        keyName = "Fifteen";
                        break;
                    case "16":
                        keyName = "Sixteen";
                        break;
                    case "17":
                        keyName = "Seventeen";
                        break;
                    case "18":
                        keyName = "Eighteen";
                        break;
                    case "19":
                        keyName = "Nineteen";
                        break;
                    case "20":
                        keyName = "Twenty";
                        break;
                    case "21":
                        keyName = "TwentyOne";
                        break;
                    case "22":
                        keyName = "TwentyTwo";
                        break;
                    case "23":
                        keyName = "TwentyThree";
                        break;
                    case "24":
                        keyName = "TwentyFour";
                        break;
                    case "25":
                        keyName = "TwentyFive";
                        break;
                    case "26":
                        keyName = "TwentySix";
                        break;
                    case "27":
                        keyName = "TwentySeven";
                        break;
                    case "28":
                        keyName = "TwentyEight";
                        break;
                    case "29":
                        keyName = "TwentyNine";
                        break;
                    case "30":
                        keyName = "Thirty";
                        break;
                    default:
                        keyName="";
                        break;
                }
                #region
                /* using (var connection = ConnectionMultiplexer.Connect(configurationOptions))
                 {
                     // 获取数据库
                     var db = connection.GetDatabase();
                     // 获取键的值
                     var myValue = db.StringGet("signal");

                     Console.WriteLine($"The value of signal is: {myValue}");
                     return Ok(myValue);
                 }*/
                #endregion
                // 获取键的值
                var myValue = "";
                Console.WriteLine($"The value of signa is: {keyName}");
                if (keyName == null || keyName == "")
                {
                    myValue = await _distributedCache.GetStringAsync("signa");

                    Console.WriteLine($"The value of signa is: {keyName}");
                }
                else
                {
                    myValue = await _distributedCache.GetStringAsync(keyName);

                    Console.WriteLine($"The value of signa is: {keyName}");
                }
                if (myValue == null)
                {
                    myValue = "{}";
                }

                return Ok(myValue);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
