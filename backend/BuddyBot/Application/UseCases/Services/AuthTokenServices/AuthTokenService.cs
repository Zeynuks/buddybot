using Microsoft.Extensions.Options;
using Application.Interfaces;
using Application.Options;
using Application.Results;
using Application.Tokens.CreateToken;
using Domain.Enums;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Services.AuthTokenServices;
public class AuthTokenService(
    IUserRepository userRepository,
    IUserAuthTokenRepository userAuthTokenRepository,
    ITokenCreator tokenCreator,
    IOptions<JwtOptions> jwtOptions,
    IUnitOfWork unitOfWork )
    : IAuthTokenService
{
    public async Task<Result<TokenDto>> GenerateTokens( int userId )
    {
        UserAuthToken? existingToken = await userAuthTokenRepository.GetByUserId( userId );
        if ( existingToken is not null )
        {
            await userAuthTokenRepository.Delete( existingToken );
        }

        List<PermissionName> permissions =await userRepository.GetUserPermissions( userId );

        string accessToken = tokenCreator.GenerateAccessToken( userId, permissions );
        string refreshToken = tokenCreator.GenerateRefreshToken();
        DateTime refreshTokenExpiryDate = DateTime.UtcNow.AddDays( jwtOptions.Value.RefreshTokenValidityInDays );

        UserAuthToken newToken = new UserAuthToken( userId, refreshToken, refreshTokenExpiryDate );
        userAuthTokenRepository.Add( newToken );

        await unitOfWork.CommitAsync();

        TokenDto result = new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return Result<TokenDto>.FromSuccess( result );
    }
}
