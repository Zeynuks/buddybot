using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class OnboardingAccessRequestConfiguration : IEntityTypeConfiguration<OnboardingAccessRequest>
{
    public void Configure( EntityTypeBuilder<OnboardingAccessRequest> builder )
    {
        builder.ToTable( nameof( OnboardingAccessRequest ) );
        builder.HasKey( oar => oar.CandidateId );

        builder.Property( o => o.CreatedAtUtc )
               .IsRequired();

        builder.Property( o => o.RequestOutcome )
               .IsRequired()
               .HasConversion<string>();

        builder.HasOne( o => o.Candidate )
               .WithOne( u => u.OnboardingAccessRequest )
               .HasForeignKey<OnboardingAccessRequest>( o => o.CandidateId )
               .IsRequired();

        builder.Property( oar => oar.IsDeleted )
               .IsRequired()
               .HasDefaultValue( false );

        builder.HasQueryFilter( oar => !oar.IsDeleted );
    }
}
