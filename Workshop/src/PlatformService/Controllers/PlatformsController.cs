namespace PlatformService.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Common.Controllers;
    using Common.Messages;

    using Microsoft.AspNetCore.Mvc;

    using PlatformService.Models;
    using PlatformService.Services;
    using PlatformService.Services.AsyncDataServices;
    using PlatformService.Services.SyncDataServices.Http;

    using static PlatformService.Infrastructure.ApiConstants;

    public class PlatformsController : ApiController
    {
        private readonly ICommandDataClient commandClient;
        private readonly IMapper mapper;
        private readonly IMessageBusClient messageBusClient;
        private readonly IPlatformsService platformService;

        public PlatformsController(
            ICommandDataClient commandClient,
            IMapper mapper,
            IMessageBusClient messageBusClient,
            IPlatformsService platformService)
        {
            this.commandClient = commandClient;
            this.mapper = mapper;
            this.messageBusClient = messageBusClient;
            this.platformService = platformService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformRead>>> GetPlatforms()
            => (await this.platformService.GetAllPlatforms<PlatformRead>()).ToList();

        [HttpGet(WithId, Name = nameof(GetPlatform))]
        public async Task<ActionResult<PlatformRead>> GetPlatform(int id)
            => this.OkOrNotFound(await this.platformService.GetPlatformById(id));

        [HttpPost]
        public async Task<ActionResult<PlatformRead>> CreatePlatform(PlatformCreate model)
        {
            var platform = await this.platformService.CreatePlatform(model);

            // Send Sync Message
            await this.commandClient.SendPlatformToCommand(platform);

            var message = this.mapper.Map<PlatformPublished>(platform);
            message.Event = nameof(PlatformPublished);

            // Send Async Message
            this.messageBusClient.PublishNewPlatform(message);

            return this.CreatedAtRoute(
                nameof(this.GetPlatform),
                new { Id = platform.Id },
                platform);
        }
    }
}
