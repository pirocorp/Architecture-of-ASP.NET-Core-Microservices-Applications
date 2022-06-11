namespace Common.Controllers
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;

    using static Common.Infrastructure.ApiConstants;

    public abstract class HomeControllerBase : ApiController
    {
        [Route(ProductionErrorRoute)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() => this.Problem();

        [Route(DevelopmentErrorRoute)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return this.NotFound();
            }

            var exceptionHandlerFeature =
                this.HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            return this.Problem(
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);
        }
    }
}
