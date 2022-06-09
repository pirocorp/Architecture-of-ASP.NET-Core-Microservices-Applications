namespace CommandService.Infrastructure
{
    using AutoMapper;

    using CommandService.Data.Models;
    using CommandService.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Command, CommandRead>();
            this.CreateMap<CommandCreate, Command>();

            this.CreateMap<Platform, PlatformRead>();
        }
    }
}
