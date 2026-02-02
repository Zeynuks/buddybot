using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;
public class CandidateProcessConfiguration : IEntityTypeConfiguration<CandidateProcess>
{
    public void Configure( EntityTypeBuilder<CandidateProcess> builder )
    {
        builder.ToTable( nameof( CandidateProcess ) );
        builder.HasKey( cp => new { cp.CandidateId, cp.ProcessKind } );

        builder.Property( cp => cp.ProcessKind )
               .IsRequired()
               .HasConversion<string>();


        builder.Property( cp => cp.CurrentStep )
               .IsRequired()
               .HasConversion<string>();

        builder.Property( cp => cp.IsActive )
               .IsRequired();
    }
}
