using Domain.Entities;
using Domain.Enums;
using Domain.Filters;

namespace Application.Filters.Users;
public class UserProcessKindFilter : IFilter<User>
{
    public ProcessKind? ProcessKind { get; set; }

    public IQueryable<User> Apply( IQueryable<User> query )
    {
        if ( ProcessKind.HasValue )
        {
            query = query.Where( u =>
                u.CandidateProcesses != null &&
                u.CandidateProcesses.Any( cp => cp.IsActive && cp.ProcessKind == ProcessKind.Value )
            );
        }
        return query;
    }
}
