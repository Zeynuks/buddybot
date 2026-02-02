using Domain.Entities;
using Domain.Filters;

namespace Domain.Repositories;
public interface ITeamRepository : IBaseRepository<Team>
{
    Task<int> CountFilteredTeams( IEnumerable<IFilter<Team>> filters );
    Task<List<Team>> GetFilteredTeams( IEnumerable<IFilter<Team>> filters );
    Task<List<Team>> GetTeamsLookup( IEnumerable<IFilter<Team>> filters );
}
