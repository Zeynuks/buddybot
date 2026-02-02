using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Departments;
public class DepartmentSearchFilter : IFilter<Department>
{
    public string? SearchTerm { get; set; }

    public IQueryable<Department> Apply( IQueryable<Department> query )
    {
        if ( !string.IsNullOrEmpty( SearchTerm ) )
        {
            string lowerSearchTerm = SearchTerm.ToLower();
            query = query.Where( d => d.Name.ToLower().Contains( lowerSearchTerm ) );
        }
        return query;
    }
}
