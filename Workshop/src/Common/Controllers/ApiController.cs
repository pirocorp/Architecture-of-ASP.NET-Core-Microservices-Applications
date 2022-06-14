namespace Common.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected ActionResult<T> OkOrNotFound<T>(T? result)
            => result ?? (ActionResult<T>)this.NotFound();
    }
}
