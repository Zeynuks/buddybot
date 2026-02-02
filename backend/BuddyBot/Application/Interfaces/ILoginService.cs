using Application.Results;
using Application.UseCases.Services;

namespace Application.Interfaces;
public interface ILoginService
{
    Task<Result<TokenDto>> Login( string login, string password );
}
