using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Presentation.Dtos;
using System.Linq;

namespace CisApi.Presentation.MappingProfiles;

/// <summary>
/// Contains AutoMapper profiles for the presentation layer.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// Configures mappings between domain models and DTOs.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Topic, TopicResponseDto>()
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.Ideas, opt => opt.MapFrom(src => src.Ideas));
        
        CreateMap<User, UserDto>();

        CreateMap<Idea, IdeaDto>();

        CreateMap<Idea, IdeaResponseDto>();
        
    }
}
