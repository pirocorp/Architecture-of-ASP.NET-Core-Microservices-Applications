namespace CommandService.Infrastructure.ConfigurationOptions
{
    public class GrpcOptions
    {
        public const string Grpc = "Grpc";

        public GrpcEndpoints Endpoints { get; set; } = new ();
    }
}
