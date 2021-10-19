namespace CarRentalSystem.Common.Infrastructure
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using GreenPipes;
    using Hangfire;
    using Hangfire.SqlServer;
    using MassTransit;
    using Messages;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Model;
    using Polly;
    using Refit;
    using Services.Identity;
    using Services.Messages;
    using Swagger;

    using static InfrastructureConstants;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebService<TDbContext>(
            this IServiceCollection services, 
            IConfiguration configuration,
            bool databaseHealthChecks = true,
            bool messagingHealthChecks = true,
            string serviceVersion = "v1")
            where TDbContext : DbContext
        {
            var connectionString = configuration.GetDefaultConnectionString();
            CreateDatabase(connectionString);

            services
                .AddDatabase<TDbContext>(configuration)
                .AddApplicationSettings(configuration)
                .AddTokenAuthentication(configuration)
                .AddHealth(configuration, databaseHealthChecks, messagingHealthChecks)
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
                    => options.UseSqlServer(
                        configuration.GetDefaultConnectionString(), 
                        // Sometimes the database may not be available for a couple of seconds
                        // The “Retries with exponential back off” solution works great in this case
                        // Entity Framework & SQL Server has built-in features to support connection retries
                        sqlOptions => sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null)));

        public static IServiceCollection AddApplicationSettings(
            this IServiceCollection services, 
            IConfiguration configuration)
            => services
                .Configure<ApplicationSettings>(
                    configuration.GetSection(nameof(ApplicationSettings)),
                    config => config.BindNonPublicProperties = true);

        public static IServiceCollection AddTokenAuthentication(
            this IServiceCollection services, 
            IConfiguration configuration,
            JwtBearerEvents events = null)
        {
            services
                .AddHttpContextAccessor()
                .AddScoped<ICurrentUserService, CurrentUserService>();

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
                .ConfigureHttpClient(Configurator)
                // If service is unavailable for couple of seconds.
                // Retry 5 times with exponential back off
                .AddTransientHttpErrorPolicy(policy 
                    => policy
                        .OrResult(result => result.StatusCode == HttpStatusCode.NotFound)
                        .WaitAndRetryAsync(10, retry => TimeSpan.FromSeconds(Math.Pow(2, retry))))
                // Circuit Breaker after 5 failed retries stop for 30 second before continue.
                .AddTransientHttpErrorPolicy(policy 
                    => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            return services;
        }

        public static IServiceCollection AddHealth(
            this IServiceCollection services,
            IConfiguration configuration,
            bool databaseHealthChecks = true,
            bool messagingHealthChecks = true)
        {
            var healthChecks = services.AddHealthChecks();

            if (databaseHealthChecks)
            {
                healthChecks
                    .AddSqlServer(configuration.GetDefaultConnectionString());
            }

            if (messagingHealthChecks)
            {
                var messageQueueSettings = GetMessageQueueSettings(configuration);

                var messageQueueConnectionString =
                    $"amqp://{messageQueueSettings.UserName}:{messageQueueSettings.Password}@{messageQueueSettings.Host}/";

                healthChecks
                    .AddRabbitMQ(rabbitConnectionString: messageQueueConnectionString);
            }

            return services;
        }

        public static IServiceCollection AddMessaging(
            this IServiceCollection services,
            IConfiguration configuration,
            bool usePolling = true,
            params Type[] consumers)
        {
            services
                .AddTransient<IPublisher, Publisher>()
                .AddTransient<IMessageService, MessageService>();

            var messageQueueSettings = GetMessageQueueSettings(configuration);

            services
                .AddMassTransit(mt =>
                {
                    consumers.ForEach(consumer => mt.AddConsumer(consumer));

                    mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rmq =>
                    {
                        rmq.Host(messageQueueSettings.Host, host =>
                        {
                            host.Username(messageQueueSettings.UserName);
                            host.Password(messageQueueSettings.Password);
                        });

                        consumers.ForEach(consumer
                            => rmq.ReceiveEndpoint(
                                consumer.FullName,
                                endpoint =>
                                {
                                    // Good prefetch count is number of tasks that can be processed times 2.
                                    endpoint.PrefetchCount = 12; // Number of CPUs is default
                                    endpoint.UseMessageRetry(x => x.Interval(10, 500));
                                    endpoint.ConfigureConsumer(bus, consumer);
                                }));
                    }));
                })
                .AddMassTransitHostedService();

            if (usePolling)
            {
                services.AddMessagesHostedService(configuration);
            }

            return services;
        }

        private static MessageQueueSettings GetMessageQueueSettings(IConfiguration configuration)
        {
            var settings = configuration.GetSection(nameof(MessageQueueSettings));

            return new MessageQueueSettings(
                settings.GetValue<string>(nameof(MessageQueueSettings.Host)),
                settings.GetValue<string>(nameof(MessageQueueSettings.UserName)),
                settings.GetValue<string>(nameof(MessageQueueSettings.Password)));
        }

        private static IServiceCollection AddMessagesHostedService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetCronJobsConnectionString();
            CreateDatabase(connectionString);

            // Add Hangfire services.
            services
                .AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(
                        configuration.GetDefaultConnectionString(),
                        new SqlServerStorageOptions()
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            DisableGlobalLocks = true
                        }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            // Register MessagesHostedService IHostedService in IoC Container.
            // And asp.net core will start it.
            services.AddHostedService<MessagesHostedService>();

            return services;
        }

        private static void CreateDatabase(string connectionString)
        {
            var dbName = connectionString
                .Split(";")[1]
                .Split("=")[1];

            using var connection = new SqlConnection(connectionString.Replace(dbName, "master"));

            connection.Open();

            using var command = new SqlCommand(
                $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{dbName}') create database [{dbName}];",
                connection);

            command.ExecuteNonQuery();
        }
    }
}
