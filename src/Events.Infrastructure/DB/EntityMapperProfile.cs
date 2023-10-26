using AutoMapper;

namespace Events.Infrastructure.DB
{
    public class EntityMapperProfile : Profile
    {
        public EntityMapperProfile()
        {
            CreateMap<Core.Events.Model.Event, Entities.Event>();
            CreateMap<Entities.Event, Core.Events.Model.Event>();
        }
    }
}
