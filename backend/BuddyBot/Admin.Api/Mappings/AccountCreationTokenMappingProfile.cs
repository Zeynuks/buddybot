using AutoMapper;
using Contracts.AccountCreationTokenDtos;
using Domain.Entities;
using Domain.Enums;

namespace Admin.Api.Mappings;

public class AccountCreationTokenMappingProfile : Profile
{
    public AccountCreationTokenMappingProfile()
    {
        CreateMap<AccountCreationToken, AccountCreationTokenDetailDto>()
            .ForMember( dest => dest.TokenValue,
                opt => opt.MapFrom( src => src.TokenValue ) )
            .ForMember( dest => dest.UserFirstName,
                opt => opt.MapFrom( src => src.User != null && src.User.ContactInfo != null
                    ? src.User.ContactInfo.FirstName
                    : null ) )
            .ForMember( dest => dest.UserLastName,
                opt => opt.MapFrom( src => src.User != null && src.User.ContactInfo != null
                    ? src.User.ContactInfo.LastName
                    : null ) )
            .ForMember( dest => dest.InviteLink,
                opt => opt.MapFrom( src => src.TelegramInviteLink ) )
            .ForMember( dest => dest.QrCodeBase64,
                opt => opt.MapFrom( src => src.QrCodeBase64 ) )
            .ForMember( dest => dest.Status,
                opt => opt.MapFrom( src => src.Status.ToString() ) )
            .ForMember( dest => dest.IssuedAt,
                opt => opt.MapFrom( src => src.IssuedAtUtc ) )
            .ForMember( dest => dest.ExpireDate,
                opt => opt.MapFrom( src => src.ExpireDate ) )
            .ForMember( dest => dest.ActivatedAt,
                opt => opt.MapFrom( src => src.ActivatedAtUtc ) )
            .ForMember( dest => dest.Creator,
                opt => opt.MapFrom( src => src.Creator ) )
            .ForMember( dest => dest.User,
                opt => opt.MapFrom( src => src.User ) )
            .ForMember( dest => dest.HRs,
                opt => opt.MapFrom( src =>
                    src.User != null && src.User.HRs != null
                        ? src.User.HRs
                        : new List<User>()
                ) )
            .ForMember( dest => dest.Mentors,
                opt => opt.MapFrom( src =>
                    src.User != null && src.User.Mentors != null
                        ? src.User.Mentors
                        : new List<User>()
                ) )
            .ForMember( dest => dest.Department,
                opt => opt.MapFrom( src => src.User != null && src.User.Team != null
                    ? src.User.Team.Department
                    : null ) )
            .ForMember( dest => dest.Team,
                opt => opt.MapFrom( src => src.User != null
                    ? src.User.Team
                    : null ) )
            .ForMember( dest => dest.UserRole,
                opt => opt.MapFrom( src =>
                    src.User != null && src.User.Roles != null
                        ? ( src.User.Roles.Any( r => r.RoleName == RoleName.HR || r.RoleName == RoleName.SuperHR )
                            ? "hr"
                            : src.User.Roles.Any( r => r.RoleName == RoleName.Candidate )
                                ? "candidate"
                                : "unknown" )
                        : "unknown"
                ) );

        CreateMap<AccountCreationToken, AccountCreationTokenListDto>()
            .ForMember( dest => dest.UserFirstName,
                opt => opt.MapFrom( src => src.User != null && src.User.ContactInfo != null
                    ? src.User.ContactInfo.FirstName
                    : null ) )
            .ForMember( dest => dest.UserLastName,
                opt => opt.MapFrom( src => src.User != null && src.User.ContactInfo != null
                    ? src.User.ContactInfo.LastName
                    : null ) )
            .ForMember( dest => dest.Status,
                opt => opt.MapFrom( src => src.Status ) )
            .ForMember( dest => dest.IssuedAt,
                opt => opt.MapFrom( src => src.IssuedAtUtc ) )
            .ForMember( dest => dest.ExpireDate,
                opt => opt.MapFrom( src => src.ExpireDate ) )
            .ForMember( dest => dest.ActivatedAt,
                opt => opt.MapFrom( src => src.ActivatedAtUtc ) )
            .ForMember( dest => dest.UserRole,
                opt => opt.MapFrom( src =>
                    src.User != null && src.User.Roles != null
                        ? ( src.User.Roles.Any( r => r.RoleName == RoleName.HR || r.RoleName == RoleName.SuperHR )
                            ? "hr"
                            : src.User.Roles.Any( r => r.RoleName == RoleName.Candidate )
                                ? "candidate"
                                : "unknown" )
                        : "unknown"
                ) )
            .ForMember( dest => dest.HRs,
                opt => opt.MapFrom( src =>
                    src.User != null && src.User.HRs != null
                        ? src.User.HRs
                        : new List<User>()
                ) );
    }
}