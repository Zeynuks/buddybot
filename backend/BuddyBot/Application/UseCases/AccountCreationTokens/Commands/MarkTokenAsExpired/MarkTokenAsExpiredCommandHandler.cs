using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AccountCreationTokens.Commands.MarkTokenAsExpired;
public class MarkTokenAsExpiredCommandHandler(
    IAccountCreationTokenRepository accountCreationTokenRepository,
    IUnitOfWork unitOfWork,
    ILogger<MarkTokenAsExpiredCommand> logger )
    : CommandBaseHandlerWithResult<MarkTokenAsExpiredCommand, AccountCreationToken>( logger )
{
    protected override async Task<Result<AccountCreationToken>> HandleImplAsync( MarkTokenAsExpiredCommand command )
    {
        AccountCreationToken? token = await accountCreationTokenRepository.GetByTokenValue( command.TokenValue );
        if ( token == null )
        {
            return Result<AccountCreationToken>.FromError( $"Приглашение с токеном {command.TokenValue} не найдено." );
        }

        if ( token.Status == AccountCreationTokenStatus.Expired )
        {
            return Result<AccountCreationToken>.FromError( $"Приглашение уже помечено, как истёкшое." );
        }

        token.SetStatusExpired();

        await unitOfWork.CommitAsync();

        return Result<AccountCreationToken>.FromSuccess( token );

    }
}
