namespace PlatformService.Infrastructure
{
    using AutoMapper;

    using Common.Messages;
    using Common.Protos;

    using PlatformService.Data.Models;
    using PlatformService.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Platform, PlatformRead>();

            this.CreateMap<PlatformCreate, Platform>();

            this.CreateMap<PlatformRead, PlatformPublished>();

            this.CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(
                    d => d.PlatformId,
                    opt => opt.MapFrom(s => s.Id));
        }
    }
}
