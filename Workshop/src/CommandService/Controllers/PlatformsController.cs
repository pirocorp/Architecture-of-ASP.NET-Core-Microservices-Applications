namespace CommandService.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Common.Controllers;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/c/[controller]")]
    public class PlatformsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult> TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return await Task.FromResult(this.Ok("Inbound test of from Platforms Controller"));
        }
    }
}
