using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AccountCreationTokens.Commands.DeleteToken;
internal class DeleteTokenCommandHandler(
    IAccountCreationTokenRepository accountCreationTokenRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteTokenCommand> logger )
    : CommandBaseHandlerWithResult<DeleteTokenCommand, string>( logger )
{
    protected override async Task<Result<string>> HandleImplAsync( DeleteTokenCommand command )
    {
        AccountCreationToken? token = await accountCreationTokenRepository.GetByTokenValue( command.TokenValue );
        if ( token == null )
        {
            return Result<string>.FromError( $"Токен с ID {command.TokenValue} не найден." );
        }

        if ( token.Status != AccountCreationTokenStatus.Activated && token.User is not null )
        {
            token.User.SoftDelete();
        }

        token.SoftDelete();
        await unitOfWork.CommitAsync();

        return Result<string>.FromSuccess( $"Токен с ID {command.TokenValue} был успешно удалён." );
    }
}
