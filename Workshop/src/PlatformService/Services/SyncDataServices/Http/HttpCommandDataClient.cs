namespace PlatformService.Services.SyncDataServices.Http
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Common.Infrastructure;

    using Microsoft.Extensions.Options;

    using PlatformService.Infrastructure.ConfigurationOptions;
    using PlatformService.Models;

    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient httpClient;
        private readonly CommandServiceOptions commandServiceOptions;

        public HttpCommandDataClient(
            HttpClient httpClient,
            IOptions<CommandServiceOptions> commandServiceOptions)
        {
            this.httpClient = httpClient;
            this.commandServiceOptions = commandServiceOptions.Value;
        }

        public async Task SendPlatformToCommand(PlatformRead message)
        {
            var serializedMessage = JsonSerializer.Serialize(message);

            var httpContent = new StringContent(
                serializedMessage,
                Encoding.UTF8,
                ContentType.ApplicationJson);

            var url = $"{this.commandServiceOptions.BaseAddress}{this.commandServiceOptions.Endpoints.Platforms}";

            var response = await this.httpClient.PostAsync(url, httpContent);

            var statusMessage = response.IsSuccessStatusCode
                ? "--> Sync POST to CommandService was OK!"
                : "--> Sync POST to CommandService was NOT OK!";

            Console.WriteLine(statusMessage);
        }
    }
}
