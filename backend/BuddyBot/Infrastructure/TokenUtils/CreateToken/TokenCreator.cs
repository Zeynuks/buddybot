using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Options;
using Application.Tokens.CreateToken;
using Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.TokenUtils.CreateToken;
public class TokenCreator( IOptions<JwtOptions> jwtOptions ) : ITokenCreator
{
    public string GenerateAccessToken( int userId, IEnumerable<PermissionName> permissions )
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim( nameof(userId), userId.ToString())
        };

        claims.AddRange(
            permissions.Select( p => new Claim( "Permission", p.ToString() ) )
        );

        SigningCredentials signingCredentials = new SigningCredentials(
           new SymmetricSecurityKey( Encoding.UTF8.GetBytes( jwtOptions.Value.Secret! ) ), SecurityAlgorithms.HmacSha256 );

        JwtSecurityToken token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes( jwtOptions.Value.TokenValidityInMinutes ),
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken( token );
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}