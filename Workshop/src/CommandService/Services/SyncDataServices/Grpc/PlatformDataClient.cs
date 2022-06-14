namespace CommandService.Services.SyncDataServices.Grpc
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using CommandService.Data.Models;
    using CommandService.Infrastructure.ConfigurationOptions;
    using Common.Protos;

    using GrpcNetClient;

    using Microsoft.Extensions.Options;

    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IMapper mapper;
        private readonly GrpcOptions grpcOptions;

        public PlatformDataClient(
            IOptions<GrpcOptions> grpcOptions,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.grpcOptions = grpcOptions.Value;
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            using var chanel = GrpcChannel.ForAddress(this.grpcOptions.Endpoints.Platforms);
            var client = new GrpcPlatform.GrpcPlatformClient(chanel);

            var request = new GetAllRequest();

            var response = await client.GetAllPlatformsAsync(request);

            return response.Platform.Select(r => this.mapper.Map<Platform>(r));
        }
    }
}
