using Application.CQRSInterfaces;
using Domain.Entities;
using Domain.Repositories;
using Application.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Departments.Queries.GetDepartmentById;
public class GetDepartmentByIdQueryHandler( 
    IDepartmentRepository departmentRepository,
    ILogger<GetDepartmentByIdQuery> logger )
    : QueryBaseHandler<Department, GetDepartmentByIdQuery>( logger )
{
    protected override async Task<Result<Department>> HandleImplAsync( GetDepartmentByIdQuery query )
    {
        Department? department = await departmentRepository.Get( query.Id );
        if ( department == null )
        {
            return Result<Department>.FromError( $"Отдел с ID {query.Id} не найден." );
        }

        return Result<Department>.FromSuccess( department );
    }
}
