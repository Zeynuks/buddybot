using Application.CQRSInterfaces;
using Domain.Entities;
using Domain.Repositories;
using Application.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Departments.Commands.DeleteDepartment;
public class DeleteDepartmentCommandHandler(
    IDepartmentRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteDepartmentCommand> logger )
    : CommandBaseHandlerWithResult<DeleteDepartmentCommand, string>(logger)
{
    protected override async Task<Result<string>> HandleImplAsync( DeleteDepartmentCommand command )
    {
        Department? department = await repository.Get( command.Id );
        if ( department == null )
        {
            return Result<string>.FromError( $"Отдел с ID {command.Id} не найден." );
        }

        if ( department.Teams?.Any( t => !t.IsDeleted ) == true )
        {
            return Result<string>.FromError( $"Нельзя удалить отдел, пока в нём есть команды." );
        }

        department.SoftDelete();
        await unitOfWork.CommitAsync();

        return Result<string>.FromSuccess( $"Отдел с ID {command.Id} был успешно удалён." );
    }
}
