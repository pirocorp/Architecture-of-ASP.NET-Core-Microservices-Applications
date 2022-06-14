namespace CommandService.Infrastructure
{
    using AutoMapper;

    using CommandService.Data.Models;
    using CommandService.Models;

    using Common.Messages;
    using Common.Protos;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Command, CommandRead>();
            this.CreateMap<CommandCreate, Command>();

            this.CreateMap<Platform, PlatformRead>();

            this.CreateMap<PlatformPublished, Platform>()
                .ForMember(
                    d => d.ExternalId,
                    opt => opt.MapFrom(s => s.Id))
                .ForMember(
                    d => d.Id,
                    opt => opt.MapFrom(s => default(int)));

            this.CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(
                    d => d.ExternalId,
                    opt => opt.MapFrom(s => s.PlatformId))
                .ForMember(
                    d => d.Commands,
                    opt => opt.Ignore());
        }
    }
}
