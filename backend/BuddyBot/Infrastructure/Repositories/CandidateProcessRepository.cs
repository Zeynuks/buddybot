using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class CandidateProcessRepository : BaseRepository<CandidateProcess>, ICandidateProcessRepository
{
    public CandidateProcessRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public async Task<CandidateProcess?> Get( int candidateId, ProcessKind processKind )
    {
        return await _dbContext.Set<CandidateProcess>()
            .Include( cp => cp.Candidate )
            .FirstOrDefaultAsync( cp => cp.CandidateId == candidateId && cp.ProcessKind == processKind );
    }

    public async Task<CandidateProcess?> GetActive( int candidateId )
    {
        return await _dbContext.Set<CandidateProcess>()
            .Include( cp => cp.Candidate )
            .FirstOrDefaultAsync( cp => cp.CandidateId == candidateId && cp.IsActive );
    }

    public async Task<List<CandidateProcess>> GetAll( int candidateId )
    {
        return await _dbContext.Set<CandidateProcess>()
            .Where( cp => cp.CandidateId == candidateId )
            .ToListAsync();
    }
}
