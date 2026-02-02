using AutoMapper;
using Contracts.DepartmentDtos;
using Domain.Entities;

namespace Admin.Api.Mappings;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Department, DepartmentLookupDto>();

        CreateMap<Department, DepartmentListDto>()
            .ForMember( dest => dest.TeamCount, 
                opt => opt.MapFrom( src => src.Teams.Count ) )
            .ForMember( dest => dest.IsVideoGreetingUploaded, 
                opt => opt.MapFrom( src => !string.IsNullOrEmpty( src.HeadVideoUrl ) ) );

        CreateMap<Department, DepartmentDetailDto>()
                .ForMember( dest => dest.Teams, 
                    opt => opt.MapFrom( src => src.Teams ) );
    }
}
