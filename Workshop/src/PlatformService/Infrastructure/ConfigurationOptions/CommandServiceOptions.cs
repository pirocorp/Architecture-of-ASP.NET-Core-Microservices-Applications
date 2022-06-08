namespace PlatformService.Infrastructure.ConfigurationOptions
{
    public class CommandServiceOptions
    {
        public const string CommandService = "CommandService";

        public string BaseAddress { get; set; } = string.Empty;

        public CommandServiceEndpoints Endpoints { get; set; } = new ();
    }
}
