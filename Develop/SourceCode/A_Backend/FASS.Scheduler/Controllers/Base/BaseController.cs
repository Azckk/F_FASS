using FASS.Scheduler.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Scheduler.Controllers.Base
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [TypeFilter(typeof(AuthorizeActionAttribute))]
    [TypeFilter(typeof(ActionLogAttribute))]
    public class BaseController : ControllerBase
    {

    }
}
