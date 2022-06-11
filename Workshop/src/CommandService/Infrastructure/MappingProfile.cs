namespace CommandService.Infrastructure
{
    using AutoMapper;

    using CommandService.Data.Models;
    using CommandService.Models;

    using Common.Messages;

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
                    opt => opt.MapFrom(s => s.Id));
        }
    }
}
