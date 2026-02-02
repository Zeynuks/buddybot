#pragma warning disable CS8602
using Application.Filters;
using Domain.Entities;
using Domain.Enums;
using Domain.Filters;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public async Task<int> CountFilteredFeedbacks( IEnumerable<IFilter<Feedback>> filters )
    {
        IQueryable<Feedback> query = _dbContext.Set<Feedback>();
        query = query.ApplyFilters( filters );
        return await query.CountAsync();
    }

    public async Task<Feedback?> GetDraftFeedback( int candidateId, ProcessKind processKind )
    {
        return await _dbContext.Set<Feedback>()
            .Where( f => f.CandidateId == candidateId && f.ProcessKind == processKind && f.State == FeedbackState.Draft )
            .OrderByDescending( f => f.CreatedAtUtc )
            .FirstOrDefaultAsync();
    }

    public async Task<List<Feedback>> GetFilteredFeedbacks( IEnumerable<IFilter<Feedback>> filters )
    {
        IQueryable<Feedback> query = _dbContext.Set<Feedback>()
            .Include( f => f.Candidate ).ThenInclude(u => u.ContactInfo)
            .Include( f => f.Candidate ).ThenInclude( u => u.Team ).ThenInclude( t => t.Department )
            .Include( f => f.Candidate ).ThenInclude( u => u.HRs ).ThenInclude( u => u.ContactInfo )
            .Include( f => f.Candidate ).ThenInclude( u => u.Mentors ).ThenInclude( u => u.ContactInfo );
        
        query = query.OrderByDescending( f => f.CreatedAtUtc );
        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }

    public override async Task<List<Feedback>> GetAll()
    {
        return await _dbContext.Set<Feedback>()
            .Include( f => f.Candidate ).ThenInclude( u => u.ContactInfo )
            .OrderByDescending( f => f.CreatedAtUtc )
            .ToListAsync();
    }
}
#pragma warning restore CS8602