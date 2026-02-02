using Domain.Entities;
using Infrastructure.Foundation.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Foundation.Database;
public class BuddyBotDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public BuddyBotDbContext ( DbContextOptions<BuddyBotDbContext> options, IConfiguration configuration )
        : base( options )
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
        if ( !optionsBuilder.IsConfigured )
        {
            var connectionString = _configuration.GetConnectionString( "DefaultConnection" );
            optionsBuilder.UseNpgsql( connectionString );
        }
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.ApplyConfiguration( new CityConfiguration() );
        modelBuilder.ApplyConfiguration( new CountryConfiguration() );
        modelBuilder.ApplyConfiguration( new DepartmentConfiguration() );
        modelBuilder.ApplyConfiguration( new FeedbackConfiguration() );
        modelBuilder.ApplyConfiguration( new HRInfoConfiguration() );
        modelBuilder.ApplyConfiguration( new OnboardingAccessRequestConfiguration() );
        modelBuilder.ApplyConfiguration( new PermissionConfiguration() );
        modelBuilder.ApplyConfiguration( new RoleConfiguration() );
        modelBuilder.ApplyConfiguration( new TeamConfiguration() );
        modelBuilder.ApplyConfiguration( new UserConfiguration() );
        modelBuilder.ApplyConfiguration( new CandidateProcessConfiguration() );
        modelBuilder.ApplyConfiguration( new UserContactInfoConfiguration() );
        modelBuilder.ApplyConfiguration( new AccountCreationTokenConfiguration() );
        modelBuilder.ApplyConfiguration( new UserAuthTokenConfiguration() );
    }
}
