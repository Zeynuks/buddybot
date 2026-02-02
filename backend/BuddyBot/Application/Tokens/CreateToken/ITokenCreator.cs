using Domain.Enums;

namespace Application.Tokens.CreateToken;
public interface ITokenCreator
{
    string GenerateAccessToken( int userId, IEnumerable<PermissionName> permissions );
    string GenerateRefreshToken();
}
