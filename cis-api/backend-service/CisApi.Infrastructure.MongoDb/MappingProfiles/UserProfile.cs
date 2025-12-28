
using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Infrastructure.MongoDb.Entities;

namespace CisApi.Infrastructure.MongoDb.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}
