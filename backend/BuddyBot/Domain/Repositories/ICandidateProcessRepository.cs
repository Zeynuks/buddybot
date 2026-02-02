using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories;
public interface ICandidateProcessRepository : IBaseRepository<CandidateProcess>
{
    Task<CandidateProcess?> Get( int candidateId, ProcessKind processKind );
    Task<CandidateProcess?> GetActive( int candidateId );
    Task<List<CandidateProcess>> GetAll( int candidateId );
}
