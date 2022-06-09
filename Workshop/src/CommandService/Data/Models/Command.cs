namespace CommandService.Data.Models
{
    using Common.Infrastructure.Exceptions;

    public class Command
    {
        private Platform? platform;

        public int Id { get; set; }

        public string HowTo { get; set; } = string.Empty;

        public string CommandLine { get; set; } = string.Empty;

        public int PlatformId { get; set; }

        public Platform Platform
        {
            get => this.platform ?? throw new UninitializedPropertyException(nameof(this.Platform));
            set => this.platform = value;
        }
    }
}
