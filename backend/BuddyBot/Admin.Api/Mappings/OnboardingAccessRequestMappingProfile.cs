using Application.UseCases.OnboardingAccessRequests.Commands.ConfirmOnboardingAccessRequest;
using AutoMapper;
using Contracts.OnboardingAccessRequestDtos;
using Domain.Entities;

namespace Admin.Api.Mappings;

public class OnboardingAccessRequestMappingProfile : Profile
{
    public OnboardingAccessRequestMappingProfile()
    {
        CreateMap<OnboardingAccessRequest, OnboardingAccessRequestListDto>()
            .ForMember( dest => dest.FirstName,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.ContactInfo != null
                    ? src.Candidate.ContactInfo.FirstName
                    : null ) )
            .ForMember( dest => dest.LastName,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.ContactInfo != null
                    ? src.Candidate.ContactInfo.LastName
                    : null ) )
            .ForMember( dest => dest.HRs,
                opt => opt.MapFrom( src =>
                    src.Candidate != null && src.Candidate.HRs != null
                        ? src.Candidate.HRs
                        : new List<User>()
                ) )
            .ForMember( dest => dest.Mentors,
                opt => opt.MapFrom( src =>
                    src.Candidate != null && src.Candidate.Mentors != null
                        ? src.Candidate.Mentors
                        : new List<User>()
                ) )
            .ForMember( dest => dest.Department,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.Team != null
                    ? src.Candidate.Team.Department
                    : null ) )
            .ForMember( dest => dest.Team,
                opt => opt.MapFrom( src => src.Candidate != null
                    ? src.Candidate.Team
                    : null ) )
            .ForMember( dest => dest.OnboardingAccessTime,
                opt => opt.MapFrom( src => src.Candidate != null
                    ? src.Candidate.OnboardingAccessTimeUtc
                    : null ) )
            .ForMember( dest => dest.CreatedAt,
                opt => opt.MapFrom( 
                    src => src.CreatedAtUtc ) );


        CreateMap<OnboardingAccessRequest, OnboardingAccessRequestDetailDto>()
            .ForMember( dest => dest.FirstName,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.ContactInfo != null
                    ? src.Candidate.ContactInfo.FirstName
                    : null ) )
            .ForMember( dest => dest.LastName,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.ContactInfo != null
                    ? src.Candidate.ContactInfo.LastName
                    : null ) )
            .ForMember( dest => dest.HRs,
                opt => opt.MapFrom( src =>
                    src.Candidate != null && src.Candidate.HRs != null
                        ? src.Candidate.HRs
                        : new List<User>()
                ) )
            .ForMember( dest => dest.Mentors,
                opt => opt.MapFrom( src =>
                    src.Candidate != null && src.Candidate.Mentors != null
                        ? src.Candidate.Mentors
                        : new List<User>()
                ) )
            .ForMember( dest => dest.Department,
                opt => opt.MapFrom( src => src.Candidate != null && src.Candidate.Team != null
                    ? src.Candidate.Team.Department
                    : null ) )
            .ForMember( dest => dest.Team,
                opt => opt.MapFrom( src => src.Candidate != null
                    ? src.Candidate.Team
                    : null ) )
            .ForMember( dest => dest.OnboardingAccessTime,
                opt => opt.MapFrom( src => src.Candidate != null
                    ? src.Candidate.OnboardingAccessTimeUtc
                    : null ) )
            .ForMember( dest => dest.CreatedAt,
                opt => opt.MapFrom(
                    src => src.CreatedAtUtc ) );
    }
}
