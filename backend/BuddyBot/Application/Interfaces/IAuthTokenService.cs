using Application.Results;
using Application.UseCases.Services;

namespace Application.Interfaces;
public interface IAuthTokenService
{
    public Task<Result<TokenDto>> GenerateTokens( int userId );
}
