namespace CarRentalSystem.Admin.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Refit;
    using Services;
    using Services.Identity;

    using static InfrastructureConstants;

    public static class ServiceCollectionExtensions
    {
        private static ServiceEndpoints serviceEndpoints;

        // Not working because of https://github.com/reactiveui/refit/issues/717
        // Bug fixed https://github.com/reactiveui/refit/pull/925
        public static IServiceCollection AddExternalService<TService>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TService : class
        {
            if (serviceEndpoints == null)
            {
                serviceEndpoints = configuration
                    .GetSection(nameof(ServiceEndpoints))
                    .Get<ServiceEndpoints>(config => config
                        .BindNonPublicProperties = true);
            }

            var serviceName = typeof(TService)
                .Name.Substring(1)
                .Replace("Service", string.Empty);

            void Configurator(IServiceProvider serviceProvider, HttpClient client)
            {
                client.BaseAddress = new Uri(serviceEndpoints[serviceName]);

                var requestServices = serviceProvider
                    .GetService<IHttpContextAccessor>()
                    ?.HttpContext
                    ?.RequestServices;

                var currentToken = requestServices?.GetService<ICurrentTokenService>()
                    ?.Get();

                if (currentToken == null)
                {
                    return;
                }

                var authorizationHeader = new AuthenticationHeaderValue(AuthorizationHeaderValuePrefix, currentToken);
                client.DefaultRequestHeaders.Authorization = authorizationHeader;
            }

            services
                .AddRefitClient<TService>()
                .ConfigureHttpClient(Configurator);

            return services;
        }
    }
}
