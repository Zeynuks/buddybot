using AutoMapper;
using Contracts.CityDtos;
using Domain.Entities;
using Domain.Enums;

namespace Admin.Api.Mappings;

public class CityMappingProfile : Profile
{
    public CityMappingProfile()
    {
        CreateMap<City, CityLookupDto>();

        CreateMap<City, CityListDto>()
            .ForMember(
                dest => dest.CandidateCount,
                opt => opt.MapFrom( src =>
                    src.Candidates.Count( c =>
                        c.User != null &&
                        c.User.Roles.Any( r => r.RoleName == RoleName.Candidate )
                    )
                )
            )
            .ForMember(
                dest => dest.CountryName,
                opt => opt.MapFrom( src => src.Country != null ? src.Country.Name : null )
            );

        CreateMap<City, CityDetailDto>()
            .ForMember(
                dest => dest.CandidateCount,
                opt => opt.MapFrom( src =>
                    src.Candidates.Count( c =>
                        c.User != null &&
                        c.User.Roles.Any( r => r.RoleName == RoleName.Candidate )
                    )
                )
            )
            .ForMember(
                dest => dest.CountryName,
                opt => opt.MapFrom( src => src.Country != null ? src.Country.Name : null )
            );

    }
}
