using Domain.Entities;
using Domain.Enums;
using Domain.Filters;

namespace Domain.Repositories;
public interface IFeedbackRepository : IBaseRepository<Feedback>
{
    Task<int> CountFilteredFeedbacks( IEnumerable<IFilter<Feedback>> filters );
    Task<List<Feedback>> GetFilteredFeedbacks( IEnumerable<IFilter<Feedback>> filters );
    Task<Feedback?> GetDraftFeedback( int candidateId, ProcessKind processKind );
}
