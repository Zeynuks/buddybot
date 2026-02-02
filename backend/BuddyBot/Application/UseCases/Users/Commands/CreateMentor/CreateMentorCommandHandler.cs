using Application.CQRSInterfaces;
using Application.Results;
using Domain.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Domain.Enums;

namespace Application.UseCases.Users.Commands.CreateMentor;
public class CreateMentorCommandHandler(
    IUserRepository userRepository,
    ITeamRepository teamRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateMentorCommand> logger )
    : CommandBaseHandlerWithResult<CreateMentorCommand, User>( logger )
{
    protected override async Task<Result<User>> HandleImplAsync( CreateMentorCommand command )
    {
        User user = new User( DateTime.UtcNow );
        UserContactInfo contactInfo = new UserContactInfo(
            user.Id,
            command.FirstName ?? string.Empty,
            command.LastName ?? string.Empty
        );

        if ( !string.IsNullOrWhiteSpace( command.MentorPhotoUrl ) )
        {
            contactInfo.AddPhoto( command.MentorPhotoUrl );
        }

        if ( !string.IsNullOrWhiteSpace( command.MicrosoftTeamsUrl ) )
        {
            contactInfo.AddMicrosoftTeamsLink( command.MicrosoftTeamsUrl );
        }

        user.SetContactInfo( contactInfo );

        Role? mentorRole = await roleRepository.Get( RoleName.Mentor );
        if ( mentorRole == null )
        {
            return Result<User>.FromError( "Роль 'Mentor' не найдена." );
        }
        user.Roles.Add( mentorRole );

        Team? team = await teamRepository.Get( command.TeamId );
        if ( team == null )
        {
            return Result<User>.FromError( "Команда не найдена." );
        }
        team.AddMember( user );

        if ( command.IsTeamLeader )
        {
            team.AssignLeader( user );
        }

        userRepository.Add( user );
        await unitOfWork.CommitAsync();

        return Result<User>.FromSuccess( user );
    }
}
