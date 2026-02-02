using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AccountCreationTokens.Commands.RevokeToken;
public class RevokeTokenCommandHandler(
    IAccountCreationTokenRepository accountCreationTokenRepository,
    IUnitOfWork unitOfWork,
    ILogger<RevokeTokenCommand> logger )
    : CommandBaseHandlerWithResult<RevokeTokenCommand, AccountCreationToken>( logger )
{
    protected override async Task<Result<AccountCreationToken>> HandleImplAsync( RevokeTokenCommand command )
    {
        AccountCreationToken? token = await accountCreationTokenRepository.GetByTokenValue(command.TokenValue);
        if ( token == null )
        {
            return Result<AccountCreationToken>.FromError( $"Приглашение с токеном {command.TokenValue} не найдено." );
        }

        if ( token.Status == AccountCreationTokenStatus.Revoked )
        {
            return Result<AccountCreationToken>.FromError( $"Приглашение уже отозвано." );
        }

        token.SetStatusRevoked();

        await unitOfWork.CommitAsync();

        return Result<AccountCreationToken>.FromSuccess( token );
    }
}
