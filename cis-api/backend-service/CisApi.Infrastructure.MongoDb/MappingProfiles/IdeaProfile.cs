
using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Infrastructure.MongoDb.Entities;

namespace CisApi.Infrastructure.MongoDb.MappingProfiles
{
    public class IdeaProfile : Profile
    {
        public IdeaProfile()
        {

            CreateMap<IdeaEntity, Idea>();

            CreateMap<Idea, IdeaEntity>();

            CreateMap<IdeaVotesEntity, IdeaVote>().ReverseMap();
            
            CreateMap<IdeaEntity, Idea>().ReverseMap();
        }
    }
}


          


