namespace PlatformService.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ActionResult<T> ReturnOkOrNotFound<T>(T? result)
            => result ?? (ActionResult<T>)this.NotFound(result);
    }
}
