using System.IdentityModel.Tokens.Jwt;
using Application.Tokens.DecodeToken;

namespace Infrastructure.TokenUtils.DecodeToken;
public class TokenDecoder : ITokenDecoder
{
    public JwtSecurityToken? DecodeToken( string accessToken )
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken? token = handler.ReadToken( accessToken ) as JwtSecurityToken;
        return token;
    }
}
