using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Infrastructure.MongoDb.Entities;

namespace CisApi.Infrastructure.MongoDb.MappingProfiles
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
             //  Entity -> Domain
            CreateMap<TopicEntity, Topic>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id.ToString()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.Ideas, opt => opt.MapFrom(src => src.Ideas));

            //  Domain -> Entity
            CreateMap<Topic, TopicEntity>()
                .ForMember(dest => dest.id, opt => opt.Ignore()) // Mongo cria ObjectId
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.Ideas, opt => opt.MapFrom(src => src.Ideas));

            // Embedded objects
            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.login))
                .ReverseMap();

            CreateMap<IdeaEntity, Idea>().ReverseMap();

        }
    }
}