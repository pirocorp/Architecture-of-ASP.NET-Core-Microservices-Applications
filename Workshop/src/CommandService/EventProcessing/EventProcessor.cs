namespace CommandService.EventProcessing
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    using CommandService.Services;

    using Common.Messages;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// EventProcessor has a Singleton lifetime and process events.
    /// </summary>
    /// <remarks>
    /// EventProcessor has a Singleton lifetime, and IPlatformsService has transient lifetime.
    /// Injecting a service with a shorter lifespan than the top service is called
    /// Captured Dependency, is an anti-pattern and can lead to bugs. The captured dependency
    /// will have the same lifetime as the principal, e.g. longer than is supposed to.
    /// </remarks>>
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;

        /// <param name="scopeFactory">
        /// To be able to use scoped or transient services within a singleton, you must create a scope manually.
        /// A new scope can be created by injecting an IServiceScopeFactory into your singleton
        /// service (the IServiceScopeFactory is itself a singleton, which is why this works).
        /// The IServiceScopeFactory has a CreateScope method, which is used for creating
        /// new scope instances.
        /// </param>
        /// <remarks>
        /// The created scope has it's own IServiceProvider, which you can access to resolve your scoped/transient services.
        /// - Only define the scope within the method that you intend to use it.
        ///   It might be tempting to assign it to a field for reuse elsewhere in
        ///   the singleton service, but again this will lead to captive dependencies.
        /// - Wrap the scope in a using statement. This will ensure that the scope is
        ///   properly disposed of once you have finished with it.
        /// </remarks>
        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public async Task ProcessEvent(string messageString)
        {
            var eventType = DetermineEvent(messageString);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    await this.AddPlatform(messageString);
                    break;
                case EventType.Undetermined:
                default:
                    break;
            }
        }

        private static EventType DetermineEvent(string message)
        {
            var eventType = JsonSerializer.Deserialize<GenericEvent>(message);

            Enum.TryParse<EventType>(eventType?.Event, true, out var result);

            return result;
        }

        private async Task AddPlatform(string message)
        {
            var platform = JsonSerializer.Deserialize<PlatformPublished>(message);

            if (platform is null)
            {
                return;
            }

            using var scope = this.scopeFactory.CreateScope();

            var platformService = scope.ServiceProvider.GetService<IPlatformsService>()
                ?? throw new NullReferenceException($"Service for {nameof(IPlatformsService)} was not found.");

            if (await platformService.ExternalExists(platform.Id))
            {
                return;
            }

            await platformService.CreatePlatform(platform);
        }
    }
}
