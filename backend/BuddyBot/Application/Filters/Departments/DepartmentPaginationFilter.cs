using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Departments;
public class DepartmentPaginationFilter : IFilter<Department>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public IQueryable<Department> Apply( IQueryable<Department> query )
    {
        int skip = ( PageNumber - 1 ) * PageSize;
        return query.Skip( skip ).Take( PageSize );
    }
}
