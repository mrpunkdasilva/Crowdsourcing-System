
using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Infrastructure.MySQL.Entities;

namespace CisApi.Infrastructure.MySQL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}
