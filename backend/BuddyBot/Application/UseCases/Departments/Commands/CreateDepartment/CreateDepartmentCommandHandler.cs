using Application.CQRSInterfaces;
using Domain.Entities;
using Domain.Repositories;
using Application.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler(
    IDepartmentRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<CreateDepartmentCommand> logger )
    : CommandBaseHandlerWithResult<CreateDepartmentCommand, Department> (logger)
{
    protected override async Task<Result<Department>> HandleImplAsync( CreateDepartmentCommand command )
    {
        if ( string.IsNullOrWhiteSpace( command.Name ) )
        {
            return Result<Department>.FromError( "Имя отдела не может быть пустым." );
        }    

        Department? department = new Department( 
            command.Name, 
            command.HeadFirstName ?? string.Empty, 
            command.HeadLastName ?? string.Empty
        );

        if ( !string.IsNullOrWhiteSpace( command.HeadVideoUrl ) )
        {
            department.AddHeadVideo( command.HeadVideoUrl );
        }

        if ( !string.IsNullOrWhiteSpace( command.HeadMicrosoftTeamsUrl ) )
        {
            department.AddHeadMicrosoftTeamsLink( command.HeadMicrosoftTeamsUrl );
        }

        repository.Add( department );
        await unitOfWork.CommitAsync();

        return Result<Department>.FromSuccess( department);
    }
}
