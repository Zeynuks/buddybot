#pragma warning disable CS8602
using Application.Filters;
using Domain.Entities;
using Domain.Enums;
using Domain.Filters;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class OnboardingAccessRequestRepository : BaseRepository<OnboardingAccessRequest>, IOnboardingAccessRequestRepository
{
    public OnboardingAccessRequestRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public async Task<int> CountFilteredOnboardingAccessRequests( IEnumerable<IFilter<OnboardingAccessRequest>> filters )
    {
        IQueryable<OnboardingAccessRequest> query = _dbContext.Set<OnboardingAccessRequest>();
        query = query.ApplyFilters( filters );
        return await query.CountAsync();
    }

    public async Task<List<OnboardingAccessRequest>> GetFilteredOnboardingAccessRequests( IEnumerable<IFilter<OnboardingAccessRequest>> filters )
    {
        IQueryable<OnboardingAccessRequest> query = _dbContext.Set<OnboardingAccessRequest>()
            .Include( oar => oar.Candidate ).ThenInclude( oar => oar.ContactInfo )
            .Include( oar => oar.Candidate ).ThenInclude( u => u.Team ).ThenInclude( t => t.Department )
            .Include( oar => oar.Candidate ).ThenInclude( u => u.HRs ).ThenInclude( u => u.ContactInfo )
            .Include( oar => oar.Candidate ).ThenInclude( u => u.Mentors ).ThenInclude( u => u.ContactInfo );

        query = query.OrderByDescending( oar => oar.CreatedAtUtc );
        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }

    public override async Task<OnboardingAccessRequest?> Get( int candidateId )
    {
        return await _dbContext.Set<OnboardingAccessRequest>()
            .Include( oar => oar.Candidate ).ThenInclude( oar => oar.ContactInfo )
            .Include( oar => oar.Candidate ).ThenInclude( u => u.Team ).ThenInclude( t => t.Department )
            .Include( oar => oar.Candidate ).ThenInclude( u => u.HRs ).ThenInclude( u => u.ContactInfo )
            .Include( oar => oar.Candidate ).ThenInclude( u => u.Mentors ).ThenInclude( u => u.ContactInfo )
            .FirstOrDefaultAsync( oar => oar.CandidateId == candidateId );
    }

    public async Task<List<User>> GetDueCandidates( DateTime utcNow )
    {
        return await _dbContext.Set<OnboardingAccessRequest>()
            .Include( r => r.Candidate ).ThenInclude( u => u.ContactInfo )
            .Where( r => r.RequestOutcome == RequestOutcome.Scheduled && 
                    r.Candidate != null && 
                    r.Candidate.OnboardingAccessTimeUtc != null && 
                    r.Candidate.OnboardingAccessTimeUtc <= utcNow )
            .Select( r => r.Candidate! )
            .ToListAsync();
    }
}
#pragma warning restore CS8602