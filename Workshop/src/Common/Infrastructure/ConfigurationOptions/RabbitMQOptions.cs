namespace Common.Infrastructure.ConfigurationOptions
{
    public class RabbitMqOptions
    {
        public const string RabbitMq = "RabbitMQ";

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }
    }
}
