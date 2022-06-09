namespace CommandService.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CommandService.Models;
    using CommandService.Services;

    using Common.Controllers;

    using Microsoft.AspNetCore.Mvc;

    using static Infrastructure.ApiConstants;

    [Route("api/c/platforms/{platformId:int}/[controller]")]
    public class CommandsController : ApiController
    {
        private readonly ICommandsService commandsService;
        private readonly IPlatformsService platformsService;

        public CommandsController(
            ICommandsService commandsService,
            IPlatformsService platformsService)
        {
            this.commandsService = commandsService;
            this.platformsService = platformsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandRead>>> GetCommandsForPlatform(int platformId)
        {
            if (!await this.platformsService.Exists(platformId))
            {
                return this.NotFound();
            }

            return (await this.commandsService.GetAll(platformId)).ToList();
        }

        [HttpGet(CommandId, Name = nameof(GetCommandForPlatform))]
        public async Task<ActionResult<CommandRead>> GetCommandForPlatform(int platformId, int commandId)
        {
            if (!await this.platformsService.Exists(platformId))
            {
                return this.NotFound();
            }

            return this.OkOrNotFound(await this.commandsService.GetCommandForPlatform(platformId, commandId));
        }

        [HttpPost]
        public async Task<ActionResult<CommandRead>> CreateCommandForPlatform(int platformId, CommandCreate model)
        {
            var command = await this.commandsService.CreateCommandForPlatform(platformId, model);

            return this.CreatedAtRoute(
                nameof(this.GetCommandForPlatform),
                new { PlatformId = platformId, CommandId = command.Id },
                command);
        }
    }
}
