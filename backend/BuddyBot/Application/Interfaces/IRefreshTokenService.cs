using Application.Results;
using Application.UseCases.Services;

namespace Application.Interfaces;
public interface IRefreshTokenService
{
    public Task<Result<TokenDto>> RefreshToken( string refreshToken );
}