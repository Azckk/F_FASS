using FASS.Web.Api.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Web.Api.Controllers.Base
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