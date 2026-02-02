using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.Users.Commands.UpdateCandidate;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Commands.UpdateUser;
public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    ITeamRepository teamRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateUserCommand> logger) 
    : CommandBaseHandlerWithResult<UpdateUserCommand, User>( logger )
{
    protected override async Task<Result<User>> HandleImplAsync( UpdateUserCommand command )
    {
        var user = await userRepository.Get( command.Id );
        if ( user == null )
            return Result<User>.FromError( $"Пользователь с id={command.Id} не найден." );

        if ( user.ContactInfo == null )
            user.SetContactInfo( new UserContactInfo( user.Id, "", "" ) );

        if ( !string.IsNullOrWhiteSpace( command.FirstName ) )
            user.ContactInfo.SetFirstName( command.FirstName );

        if ( !string.IsNullOrWhiteSpace( command.LastName ) )
            user.ContactInfo.SetLastName( command.LastName );

        if ( !string.IsNullOrWhiteSpace( command.MicrosoftTeamsUrl ) )
            user.ContactInfo.AddMicrosoftTeamsLink( command.MicrosoftTeamsUrl );

        if ( command.PhotoUrl == null || string.IsNullOrWhiteSpace( command.PhotoUrl ) )
        {
            user.ContactInfo.RemovePhoto();
        }
        else
        {
            user.ContactInfo.AddPhoto( command.PhotoUrl );
        }

        if ( !string.IsNullOrWhiteSpace( command.VideoUrl ) )
            user.ContactInfo.AddVideo( command.VideoUrl );

        if ( command.TeamId.HasValue && user.TeamId.HasValue && user.TeamId != command.TeamId )
        {
            var oldTeam = await teamRepository.Get( user.TeamId.Value );
            if ( oldTeam != null && oldTeam.LeaderId == user.Id )
            {
                oldTeam.RemoveLeader();
            }
        }

        if ( command.TeamId.HasValue )
        {
            var team = await teamRepository.Get( command.TeamId.Value );
            if ( team == null )
                return Result<User>.FromError( "Команда не найдена." );
            user.SetTeam( team );
        }

        if ( command.HrIds != null )
        {
            var hrs = await userRepository.GetUsersByIds( command.HrIds );
            user.SetHRs( hrs );
        }
        if ( command.MentorIds != null )
        {
            var mentors = await userRepository.GetUsersByIds( command.MentorIds );
            user.SetMentors( mentors );
        }

        if ( command.OnboardingAccessTimeUtc.HasValue )
            user.SetOnboardingAccessTime( command.OnboardingAccessTimeUtc.Value );

        if ( !string.IsNullOrWhiteSpace( command.TelegramContact ) )
            user.ContactInfo.SetTelegramContact( command.TelegramContact );

        await unitOfWork.CommitAsync();

        return Result<User>.FromSuccess( user );
    }
}
