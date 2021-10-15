namespace CarRentalSystem.Common.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using MassTransit;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Model;
    using Refit;
    using Services.Identity;
    using Swagger;

    using static InfrastructureConstants;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebService<TDbContext>(
            this IServiceCollection services, 
            IConfiguration configuration,
            string serviceVersion = "v1")
            where TDbContext : DbContext
        {
            services
                .AddDatabase<TDbContext>(configuration)
                .AddApplicationSettings(configuration)
                .AddTokenAuthentication(configuration)
                .AddAutoMapperProfile(Assembly.GetCallingAssembly())
                .AddControllers();

            services
                .AddSwagger(Assembly.GetCallingAssembly().GetName().Name, serviceVersion);

            return services;
        }

        public static IServiceCollection AddDatabase<TDbContext>(
            this IServiceCollection services, 
            IConfiguration configuration)
            where TDbContext : DbContext
            => services
                .AddScoped<DbContext, TDbContext>()
                .AddDbContext<TDbContext>(options 
                    => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static IServiceCollection AddApplicationSettings(
            this IServiceCollection services, 
            IConfiguration configuration)
            => services
                .Configure<ApplicationSettings>(configuration
                    .GetSection(nameof(ApplicationSettings)));

        public static IServiceCollection AddTokenAuthentication(
            this IServiceCollection services, 
            IConfiguration configuration,
            JwtBearerEvents events = null)
        {
            var secret = configuration
                .GetSection(nameof(ApplicationSettings))
                .GetValue<string>(nameof(ApplicationSettings.Secret));

            var key = Encoding.ASCII.GetBytes(secret);

            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    if (events != null)
                    {
                        bearer.Events = events;
                    }
                });

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        public static IServiceCollection AddSwagger(
            this IServiceCollection services, 
            string serviceName, 
            string serviceVersion)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(serviceVersion, new OpenApiInfo { Title = serviceName, Version = serviceVersion });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.OperationFilter<AuthResponsesOperationFilter>();
            });

            return services;
        }

        public static IServiceCollection AddAutoMapperProfile(
            this IServiceCollection services,
            Assembly assembly)
            => services
                .AddAutoMapper(
                    (_, config) => config.AddProfile(new MappingProfile(assembly)),
                    Array.Empty<Assembly>());

        public static IServiceCollection AddExternalService<TService>(
            this IServiceCollection services,
            IConfiguration configuration,
            string baseAddress)
            where TService : class
        {
            void Configurator(IServiceProvider serviceProvider, HttpClient client)
            {
                client.BaseAddress = new Uri(baseAddress);

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

        public static IServiceCollection AddMessaging(
            this IServiceCollection services,
            params Type[] consumers)
        {
            services
                .AddMassTransit(mt =>
                {
                    consumers.ForEach(consumer => mt.AddConsumer(consumer));

                    mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rmq =>
                    {
                        rmq.Host("rabbitmq", host =>
                        {
                            host.Username("rabbitmq");
                            host.Password("rabbitmq");
                        });

                        consumers.ForEach(consumer 
                            => rmq.ReceiveEndpoint(
                                consumer.FullName, 
                                endpoint => endpoint.ConfigureConsumer(bus, consumer)));
                    }));
                })
                .AddMassTransitHostedService();

            return services;
        }
    }
}
