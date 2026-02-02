using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Departments.Queries.GetDepartmentsLookup;
public class GetDepartmentsLookupQueryHandler( 
    IDepartmentRepository departmentRepository,
    ILogger<GetDepartmentsLookupQuery> logger )
    : QueryBaseHandler<List<Department>, GetDepartmentsLookupQuery>( logger )
{
    protected override async Task<Result<List<Department>>> HandleImplAsync( GetDepartmentsLookupQuery query )
    {
        List<Department> departments = await departmentRepository.GetDepartmentsLookup();
        return Result<List<Department>>.FromSuccess( departments );
    }
}