namespace PlatformService.Services.SyncDataServices.Grpc
{
    using System.Threading.Tasks;

    using Common.Protos;

    using GrpcCore;

    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformsService platformsService;

        public GrpcPlatformService(IPlatformsService platformsService)
        {
            this.platformsService = platformsService;
        }

        public override async Task<PlatformResponse> GetAllPlatforms(
            GetAllRequest request,
            ServerCallContext context)
        {
            var platforms = await this.platformsService
                .GetAllPlatforms<GrpcPlatformModel>();

            var response = new PlatformResponse();
            response.Platform.AddRange(platforms);

            return response;
        }
    }
}
