namespace PlatformService.Services.AsyncDataServices
{
    using System;
    using System.Text;
    using System.Text.Json;

    using Common.Infrastructure.ConfigurationOptions;

    using Microsoft.Extensions.Options;

    using PlatformService.Models;
    using RabbitMQ.Client;

    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private const string ExchangeName = "trigger";

        private readonly IConnection connection;
        private readonly IModel channel;

        private bool disposed;

        public MessageBusClient(IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqOptions.Value.Host,
                Port = rabbitMqOptions.Value.Port,
            };

            this.connection = factory.CreateConnection();
            this.channel = this.connection.CreateModel();

            this.channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout);
            this.connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        public void PublishNewPlatform(PlatformPublished model)
        {
            var message = JsonSerializer.Serialize(model);

            if (this.connection.IsOpen)
            {
                this.SendMessage(message);
            }
            else
            {
                // TODO: Some retry policy
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.channel.IsOpen)
                {
                    this.channel.Close();
                    this.connection.Close();
                }

                this.channel.Dispose();
                this.connection.Dispose();
            }

            this.disposed = true;
        }

        private static void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"--> RabbitMQ connection Shutdown.");
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            this.channel.BasicPublish(ExchangeName, string.Empty, null, body);
        }
    }
}
