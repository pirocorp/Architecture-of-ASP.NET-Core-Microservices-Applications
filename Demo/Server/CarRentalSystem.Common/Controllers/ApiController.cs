namespace CarRentalSystem.Common.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected const string PathSeparator = "/";
        protected const string Id = "{id}";
    }
}