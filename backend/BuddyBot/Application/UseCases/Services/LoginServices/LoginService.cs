using Application.Interfaces;
using Application.PasswordHasher;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Services.LoginServices;
public class LoginService(
    IAuthTokenService authTokenService,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher )
    : ILoginService
{
    public async Task<Result<TokenDto>> Login( string login, string password )
    {
        User? user = await userRepository.GetByLogin( login );

        if ( user is null || user.PasswordHash is null || !passwordHasher.VerifyPassword( password, user.PasswordHash ) )
        {
            return Result<TokenDto>.FromError( "Неверное имя пользователя или пароль" );
        }

        return await authTokenService.GenerateTokens( user.Id );
    }
}