using AutoMapper;
using Contracts.FeedbackDtos;
using Domain.Entities;

namespace Admin.Api.Mappings;

public class FeedbackMappingProfile : Profile
{
    public FeedbackMappingProfile()
    {
        CreateMap<Feedback, FeedbackListDto>()
            .ForMember( dest => dest.FirstName,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.ContactInfo != null
                    ? src.Candidate.ContactInfo.FirstName
                    : null ) )
            .ForMember( dest => dest.LastName,
            opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.ContactInfo != null
                ? src.Candidate.ContactInfo.LastName
                : null ) )
            .ForMember( dest => dest.DepartmentName,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.Team != null && src.Candidate.Team.Department != null
                    ? src.Candidate.Team.Department.Name
                    : null ) )
            .ForMember( dest => dest.TeamName,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.Team != null
                    ? src.Candidate.Team.Name
                    : null ) )
            .ForMember( dest => dest.HRNames,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.HRs != null
                    ? src.Candidate.HRs.Select( hr => $"{hr.ContactInfo!.FirstName} {hr.ContactInfo.LastName}" ).ToList()
                    : new List<string>() ) )
            .ForMember( dest => dest.MentorNames,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.Mentors != null
                    ? src.Candidate.Mentors.Select( m => $"{m.ContactInfo!.FirstName} {m.ContactInfo.LastName}" ).ToList()
                    : new List<string>() ) )
            .ForMember( dest => dest.CreatedAt,
                opt => opt.MapFrom(
                    src => src.CreatedAtUtc ) );
        ;
    }
}
