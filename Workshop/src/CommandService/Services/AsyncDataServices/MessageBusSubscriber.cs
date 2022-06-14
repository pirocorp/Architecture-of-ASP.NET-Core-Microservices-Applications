namespace CommandService.Services.AsyncDataServices
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using CommandService.EventProcessing;

    using Common.Infrastructure.ConfigurationOptions;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    using static Common.Messages.MessagesConstants;

    /// <summary>
    /// Background service listening for events on message bus.
    /// </summary>
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IEventProcessor eventProcessor;
        private readonly RabbitMqOptions rabbitMqOptions;

        private IConnection connection;
        private IModel chanel;
        private string queueName = string.Empty;

        public MessageBusSubscriber(
            IOptions<RabbitMqOptions> rabbitMqOptions,
            IEventProcessor eventProcessor)
        {
            this.rabbitMqOptions = rabbitMqOptions.Value;
            this.eventProcessor = eventProcessor;

            this.InitializeRabbitMq();
        }

        /// <summary>
        /// This method is called when the <see cref="IHostedService"/> starts.
        /// </summary>
        /// <param name="stoppingToken">Means "stop this service".</param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(this.ShutDown);

            var consumer = new EventingBasicConsumer(this.chanel);

            consumer.Received += async (sender, ea) =>
            {
                var notificationMessage = Encoding.UTF8.GetString(ea.Body.ToArray());

                await this.eventProcessor.ProcessEvent(notificationMessage);
            };

            this.chanel.BasicConsume(this.queueName, true, consumer);

            return Task.CompletedTask;
        }

        private static void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"--> RabbitMQ connection Shutdown.");
        }

        [MemberNotNull(nameof(connection), nameof(chanel))]
        private void InitializeRabbitMq()
        {
            var factory = new ConnectionFactory()
            {
                HostName = this.rabbitMqOptions.Host,
                Port = this.rabbitMqOptions.Port,
            };

            this.connection = factory.CreateConnection();
            this.chanel = this.connection.CreateModel();

            this.chanel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);
            this.queueName = this.chanel.QueueDeclare().QueueName;
            this.chanel.QueueBind(this.queueName, ExchangeName, string.Empty);

            this.connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        private void ShutDown()
        {
            if (this.chanel.IsOpen)
            {
                this.chanel.Close();
            }

            if (this.connection.IsOpen)
            {
                this.connection.Close();
            }

            this.chanel.Dispose();
            this.connection.Dispose();
        }
    }
}
