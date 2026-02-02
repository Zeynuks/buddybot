using AutoMapper;
using Contracts.CountryDtos;
using Domain.Entities;

namespace Admin.Api.Mappings;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
        CreateMap<Country, CountryLookupDto>();

        CreateMap<Country, CountryListDto>()
            .ForMember( dest => dest.CitiesCount, 
                opt => opt.MapFrom( src => src.Cities.Count ) );

        CreateMap<Country, CountryDetailDto>()
            .ForMember( dest => dest.CitiesCount,
                opt => opt.MapFrom( src => src.Cities.Count ) );
    }
}
