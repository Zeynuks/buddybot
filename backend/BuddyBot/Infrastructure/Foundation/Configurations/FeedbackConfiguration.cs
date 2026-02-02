using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure( EntityTypeBuilder<Feedback> builder )
    {
        builder.ToTable( nameof( Feedback ) );
        builder.HasKey( f => f.Id );

        builder.Property( f => f.ProcessKind )
               .IsRequired()
               .HasConversion<string>();

        builder.Property( f => f.Rating)
               .IsRequired();

        builder.Property( f => f.Comment )
               .HasMaxLength( 4096 );

        builder.Property( f => f.CreatedAtUtc )
               .IsRequired();

        builder.HasOne( f => f.Candidate )
               .WithMany( u => u.Feedbacks )
               .HasForeignKey( f => f.CandidateId )
               .IsRequired();

        builder.Property( f => f.State )
               .IsRequired()
               .HasConversion<string>();

        builder.Property( f => f.IsDeleted )
               .IsRequired()
               .HasDefaultValue( false );

        builder.HasQueryFilter( f => !f.IsDeleted );
    }
}
