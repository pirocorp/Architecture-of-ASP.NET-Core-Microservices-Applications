namespace PlatformService.Controllers
{
    using Common.Controllers;

    using Microsoft.AspNetCore.Mvc;

    using PlatformService.Models;
    using PlatformService.Services;

    public class PlatformsController : ApiController
    {
        private readonly IPlatformsService platformService;

        public PlatformsController(IPlatformsService platformService)
        {
            this.platformService = platformService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformRead>>> GetPlatforms()
            => (await this.platformService.GetAllPlatforms()).ToList();

        [HttpGet("{id:int}", Name = nameof(GetPlatform))]
        public async Task<ActionResult<PlatformRead>> GetPlatform(int id)
            => this.ReturnOkOrNotFound(await this.platformService.GetPlatformById(id));

        [HttpPost]
        public async Task<ActionResult<PlatformRead>> CreatePlatform(PlatformCreate model)
        {
            var platform = await this.platformService.CreatePlatform(model);

            return this.CreatedAtRoute(
                nameof(this.GetPlatform),
                new { Id = platform.Id },
                platform);
        }
    }
}
