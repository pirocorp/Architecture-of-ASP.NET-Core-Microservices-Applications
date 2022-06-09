namespace CommandService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CommandService.Models;
    using CommandService.Services;
    using Common.Controllers;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/c/[controller]")]
    public class PlatformsController : ApiController
    {
        private readonly IPlatformsService platformsService;

        public PlatformsController(IPlatformsService platformsService)
        {
            this.platformsService = platformsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformRead>>> GetPlatforms()
            => (await this.platformsService.GetAll()).ToList();

        [HttpPost]
        public async Task<ActionResult> TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return await Task.FromResult(this.Ok("Inbound test of from Platforms Controller"));
        }
    }
}
