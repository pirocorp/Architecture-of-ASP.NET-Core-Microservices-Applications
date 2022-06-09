namespace PlatformService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common.Controllers;

    using Microsoft.AspNetCore.Mvc;

    using PlatformService.Models;
    using PlatformService.Services;
    using PlatformService.Services.SyncDataServices.Http;

    using static PlatformService.Infrastructure.ApiConstants;

    public class PlatformsController : ApiController
    {
        private readonly IPlatformsService platformService;
        private readonly ICommandDataClient commandClient;

        public PlatformsController(
            IPlatformsService platformService,
            ICommandDataClient commandClient)
        {
            this.platformService = platformService;
            this.commandClient = commandClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformRead>>> GetPlatforms()
            => (await this.platformService.GetAllPlatforms()).ToList();

        [HttpGet(WithId, Name = nameof(GetPlatform))]
        public async Task<ActionResult<PlatformRead>> GetPlatform(int id)
            => this.OkOrNotFound(await this.platformService.GetPlatformById(id));

        [HttpPost]
        public async Task<ActionResult<PlatformRead>> CreatePlatform(PlatformCreate model)
        {
            var platform = await this.platformService.CreatePlatform(model);

            try
            {
                await this.commandClient.SendPlatformToCommand(platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return this.CreatedAtRoute(
                nameof(this.GetPlatform),
                new { Id = platform.Id },
                platform);
        }
    }
}
