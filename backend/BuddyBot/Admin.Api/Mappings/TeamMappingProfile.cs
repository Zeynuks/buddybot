using AutoMapper;
using Contracts.TeamDtos;
using Domain.Entities;
using Domain.Enums;

namespace Admin.Api.Mappings;

public class TeamMappingProfile : Profile
{
    public TeamMappingProfile()
    {
        CreateMap<Team, TeamLookupDto>();

        CreateMap<Team, TeamListDto>()
            .ForMember(
                dest => dest.MemberCount,
                opt => opt.MapFrom( src =>
                    src.Members.Count( m =>
                        !m.IsDeleted &&
                        m.Roles.Any( r =>
                            r.RoleName == RoleName.HR || r.RoleName == RoleName.Mentor
                        )
                    )
                )
            )
            .ForMember(
                dest => dest.DepartmentName,
                opt => opt.MapFrom( src => src.Department != null
                    ? src.Department.Name
                    : null )
            )
            .ForMember(
                dest => dest.LeaderId,
                opt => opt.MapFrom( src => src.Leader != null
                    ? ( int? )src.Leader.Id
                    : null )
            )
            .ForMember(
                dest => dest.LeaderFirstName,
                opt => opt.MapFrom( src => src.Leader != null && src.Leader.ContactInfo != null 
                    ? src.Leader.ContactInfo.FirstName 
                    : null )
            )
            .ForMember(
                dest => dest.LeaderLastName,
                opt => opt.MapFrom( src => src.Leader != null && src.Leader.ContactInfo != null
                ? src.Leader.ContactInfo.LastName 
                : null )
            );

        CreateMap<Team, TeamDetailDto>()
            .ForMember(
                dest => dest.DepartmentId,
                opt => opt.MapFrom( src => src.Department != null
                    ? src.Department.Id
                    : 0 )
            )
            .ForMember(
                dest => dest.DepartmentName,
                opt => opt.MapFrom( src => src.Department != null
                    ? src.Department.Name
                    : null )
            )
            .ForMember(
                dest => dest.LeaderId,
                opt => opt.MapFrom( src => src.Leader != null
                    ? ( int? )src.Leader.Id
                    : null )
            );
    }
}