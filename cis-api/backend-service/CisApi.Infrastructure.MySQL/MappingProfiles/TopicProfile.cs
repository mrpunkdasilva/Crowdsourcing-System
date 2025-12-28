using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Infrastructure.MySQL.Entities;

namespace CisApi.Infrastructure.MySQL.MappingProfiles
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            // Entity -> Domain
            CreateMap<TopicEntity, Topic>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.Ideas, opt => opt.MapFrom(src => src.Ideas));

            // Domain -> Entity
            CreateMap<Topic, TopicEntity>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.Ideas, opt => opt.MapFrom(src => src.Ideas));
                
            // User ↔ UserEntity
            CreateMap<UserEntity, User>().ReverseMap();

            // Idea ↔ IdeaEntity
            CreateMap<IdeaEntity, Idea>().ReverseMap();

        }
    }
}