using Application.UseCases.Users.Commands.CreateMentor;
using Application.UseCases.Users.Commands.UpdateUser;
using AutoMapper;
using Contracts.UserDtos;
using Domain.Entities;

namespace Admin.Api.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile() 
    {
        CreateMap<User, UserLookupDto>()
            .ForMember( d => d.Id,
                opt => opt.MapFrom( src => src.Id ) )
            .ForMember( d => d.FirstName,
                opt => opt.MapFrom( src => src.ContactInfo != null
                    ? src.ContactInfo.FirstName
                    : null ) )
            .ForMember( d => d.LastName,
                opt => opt.MapFrom( src => src.ContactInfo != null
                    ? src.ContactInfo.LastName
                    : null ) )
            .ForMember( d => d.Roles,
                opt => opt.MapFrom( src => src.Roles != null
                    ? src.Roles.Select( r => r.RoleName.ToString() ).ToList()
                    : new List<string>() ) );

        CreateMap<User, UserDetailDto>()
            .ForMember( d => d.Id,
                opt => opt.MapFrom( src => src.Id ) )
            .ForMember( d => d.FirstName,
                opt => opt.MapFrom( src => src.ContactInfo != null
                    ? src.ContactInfo.FirstName
                    : null ) )
            .ForMember( d => d.LastName,
                opt => opt.MapFrom( src => src.ContactInfo != null
                    ? src.ContactInfo.LastName
                    : null ) )
            .ForMember( d => d.Roles,
                opt => opt.MapFrom( src => src.Roles != null
                    ? src.Roles.Select( r => r.RoleName.ToString() ).ToList()
                    : new List<string>() ) )
            .ForMember( d => d.TelegramId,
                opt => opt.MapFrom( src => src.ContactInfo != null
                    ? ( long? )src.ContactInfo.TelegramId
                    : null ) )
            .ForMember( d => d.TelegramContact,
                opt => opt.MapFrom( src => src.ContactInfo != null
                    ? src.ContactInfo.TelegramContact
                    : null ) )
            .ForMember( d => d.PhotoUrl, opt => opt.MapFrom( src => src.ContactInfo != null
                ? src.ContactInfo.MentorPhotoUrl
                : null ) )
            .ForMember( d => d.VideoUrl, opt => opt.MapFrom( src => src.ContactInfo != null
                ? src.ContactInfo.MentorVideoUrl
                : null ) )
            .ForMember( d => d.MicrosoftTeamsUrl, opt => opt.MapFrom( src => src.ContactInfo != null
                ? src.ContactInfo.MicrosoftTeamsUrl
                : null ) )
            .ForMember( d => d.CityId, opt => opt.MapFrom( src => src.ContactInfo != null
                ? src.ContactInfo.CityId
                : null ) )
            .ForMember( d => d.CityName, opt => opt.MapFrom( src => src.ContactInfo != null && src.ContactInfo.City != null
                ? src.ContactInfo.City.Name
                : null ) )
            .ForMember( d => d.HRs,
                opt => opt.MapFrom( src => src.HRs != null
                    ? src.HRs.Select( hr => hr ).ToList()
                    : new List<User>() ) )
            .ForMember( d => d.Mentors,
                opt => opt.MapFrom( src => src.Mentors != null
                    ? src.Mentors.Select( m => m ).ToList()
                    : new List<User>() ) )
            .ForMember( d => d.Team,
                opt => opt.MapFrom( src => src.Team ) )
            .ForMember( d => d.CreatedAt,
                opt => opt.MapFrom( src => src.CreatedAtUtc ) )
            .ForMember( d => d.IsTeamLeader,
                opt => opt.MapFrom( src =>
                    src.Team != null && src.Team.LeaderId == src.Id
                ) )
            .ForMember(
                d => d.ActiveProcessKind,
                opt => opt.MapFrom( ( src, d ) =>
                    src.CandidateProcesses != null
                        ? src.CandidateProcesses.FirstOrDefault( p => p.IsActive )?.ProcessKind.ToString()
                        : null
                )
            )
            .ForMember( d => d.IsActivated,
                opt => opt.MapFrom( src =>
                    src.ContactInfo != null && src.ContactInfo.TelegramId > 0 ) )
            .ForMember( d => d.IsOnboardingAccessGranted,
                opt => opt.MapFrom( src =>
                    ( src.OnboardingAccessTimeUtc.HasValue && src.OnboardingAccessTimeUtc.Value <= DateTime.UtcNow )
                    || src.OnboardingAccessRequest != null
                )
            );

        CreateMap<User, CurrentUserDto>()
            .ForMember( d => d.Id,
                opt => opt.MapFrom( src => src.Id ) )
            .ForMember( d => d.FirstName,
                opt => opt.MapFrom( src => src.ContactInfo != null
                    ? src.ContactInfo.FirstName
                    : null ) )
            .ForMember( d => d.LastName,
                opt => opt.MapFrom( src => src.ContactInfo != null
                    ? src.ContactInfo.LastName
                    : null ) )
            .ForMember( d => d.Roles,
                opt => opt.MapFrom( src => src.Roles != null
                    ? src.Roles.Select( r => r.RoleName.ToString() ).ToList()
                    : new List<string>() ) );
    }
}
