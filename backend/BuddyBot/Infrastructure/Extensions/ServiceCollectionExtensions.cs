using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Infrastructure.Foundation;
using Infrastructure.Repositories;
using Infrastructure.FileUtils;
using Infrastructure.TokenUtils.CreateToken;
using Infrastructure.TokenUtils.DecodeToken;
using Infrastructure.TokenUtils.VerificationToken;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Application;
using Application.Interfaces;
using Application.Tokens.CreateToken;
using Application.Tokens.DecodeToken;
using Application.Tokens.VerificationToken;
using Application.PasswordHasher;

namespace Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration configuration, bool includeFileStorage = true )
    {
        services.AddDbContext<BuddyBotDbContext>( options =>
            options.UseNpgsql(
                configuration.GetConnectionString( "DefaultConnection" ),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior( QuerySplittingBehavior.SplitQuery )
            )
        );

        services.AddScoped<ICandidateProcessRepository, CandidateProcessRepository>();
        services.AddScoped<IHRInfoRepository, HRInfoRepository>();
        services.AddScoped<IOnboardingAccessRequestRepository, OnboardingAccessRequestRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserContactInfoRepository, UserContactInfoRepository>();
        services.AddScoped<IUserAuthTokenRepository, UserAuthTokenRepository>();
        services.AddScoped<IAccountCreationTokenRepository, AccountCreationTokenRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ITokenCreator, TokenCreator>();
        services.AddScoped<ITokenDecoder, TokenDecoder>();
        services.AddScoped<ITokenSignatureVerificator, TokenSignatureVerificator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        if ( includeFileStorage )
        {
            services.AddScoped<IFileStorageService, LocalFileStorage>();
        }

        return services;
    }
}
