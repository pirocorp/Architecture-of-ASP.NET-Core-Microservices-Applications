namespace PlatformService.Infrastructure
{
    using AutoMapper;

    using PlatformService.Data.Models;
    using PlatformService.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Platform, PlatformRead>();

            this.CreateMap<PlatformCreate, Platform>();

            this.CreateMap<PlatformRead, PlatformPublished>();
        }
    }
}
