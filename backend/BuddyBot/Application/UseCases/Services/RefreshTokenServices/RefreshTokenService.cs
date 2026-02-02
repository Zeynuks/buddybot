using Application.Interfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Services.RefreshTokenServices;
public class RefreshTokenService(
    IUserAuthTokenRepository userAuthTokenRepository,
    IAuthTokenService authTokenService )
    : IRefreshTokenService
{
    public async Task<Result<TokenDto>> RefreshToken( string refreshToken )
    {
        UserAuthToken? userAuthToken = await userAuthTokenRepository.GetByRefreshToken( refreshToken );

        if ( userAuthToken is null || userAuthToken.ExpireDate < DateTime.UtcNow )
        {
            return Result<TokenDto>.FromError( "Недействительный или истекший токен" );
        }

        return await authTokenService.GenerateTokens( userAuthToken.UserId );
    }
}