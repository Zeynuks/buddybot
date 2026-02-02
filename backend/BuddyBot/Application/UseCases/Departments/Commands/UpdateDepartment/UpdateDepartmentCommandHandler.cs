using Application.CQRSInterfaces;
using Domain.Entities;
using Domain.Repositories;
using Application.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Departments.Commands.UpdateDepartment;
public class UpdateDepartmentCommandHandler(
    IDepartmentRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateDepartmentCommand> logger )
    : CommandBaseHandlerWithResult<UpdateDepartmentCommand, Department>( logger )
{
    protected override async Task<Result<Department>> HandleImplAsync( UpdateDepartmentCommand command )
    {
        Department? department = await repository.Get( command.Id );
        if ( department == null )
        {
            return Result<Department>.FromError( $"Отдел с ID {command.Id} не найден." );
        }

        if ( !string.IsNullOrWhiteSpace( command.Name ) )
        {
            department.Name = command.Name;
        }

        if ( !string.IsNullOrWhiteSpace( command.HeadFirstName ) )
        {
            department.HeadFirstName = command.HeadFirstName;
        }

        if ( !string.IsNullOrWhiteSpace( command.HeadLastName ) )
        {
            department.HeadLastName = command.HeadLastName;
        }

        if ( !string.IsNullOrWhiteSpace( command.HeadMicrosoftTeamsUrl ) )
        {
            department.AddHeadMicrosoftTeamsLink( command.HeadMicrosoftTeamsUrl );
        }

        department.AddHeadVideo( command.HeadVideoUrl );

        await unitOfWork.CommitAsync();

        return Result<Department>.FromSuccess( department );
    }
}
