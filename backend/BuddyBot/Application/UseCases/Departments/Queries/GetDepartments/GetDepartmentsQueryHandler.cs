using Application.CQRSInterfaces;
using Domain.Entities;
using Domain.Repositories;
using Application.Results;
using Domain.Filters;
using Application.Filters.Departments;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Departments.Queries.GetDepartments;
public class GetDepartmentsQueryHandler( 
    IDepartmentRepository departmentRepository,
    ILogger<GetDepartmentsQuery> logger )
    : QueryBaseHandler<PagedResult<Department>, GetDepartmentsQuery>(logger)
{
    protected override async Task<Result<PagedResult<Department>>> HandleImplAsync( GetDepartmentsQuery query )
    {
        List<IFilter<Department>> filters = new List<IFilter<Department>>
        {
            new DepartmentSearchFilter { SearchTerm = query.SearchTerm },
        };

        int totalCount = await departmentRepository.CountFilteredDepartments( filters );
        filters.Add( new DepartmentPaginationFilter { PageNumber = query.PageNumber, PageSize = query.PageSize });

        List<Department> departments = await departmentRepository.GetFilteredDepartments( filters );

        return Result<PagedResult<Department>>.FromSuccess( new PagedResult<Department>
        {
            Items = departments,
            TotalCount = totalCount
        } );
    }
}
