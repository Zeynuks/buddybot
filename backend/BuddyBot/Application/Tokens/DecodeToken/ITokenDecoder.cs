using System.IdentityModel.Tokens.Jwt;

namespace Application.Tokens.DecodeToken;
public interface ITokenDecoder
{
    JwtSecurityToken? DecodeToken( string accessToken );
}