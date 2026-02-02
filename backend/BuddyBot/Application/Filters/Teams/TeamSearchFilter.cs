using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Teams;
public class TeamSearchFilter : IFilter<Team>
{
    public string? SearchTerm { get; set; }

    public IQueryable<Team> Apply( IQueryable<Team> query )
    {
        if ( !string.IsNullOrEmpty( SearchTerm ) )
        {
            string lowerSearchTerm = SearchTerm.ToLower();
            query = query.Where( t => t.Name.ToLower().Contains( lowerSearchTerm ) );
        }
        return query;
    }
}
