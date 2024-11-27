using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Server.Models;

namespace Server.Services;

public sealed class JwtTokens
{
    private SecurityKey _jwtSecretKey;
    private IConfiguration _configuration;

    public JwtTokens(SecurityKey jwtSecretKey, IConfiguration configuration)
    {
        _jwtSecretKey = jwtSecretKey;
        _configuration = configuration;
    }

    // https://github.com/adityaoberai/JWTAuthSample/blob/main/JWTAuth/Business/AuthService/Implementation/AuthService.cs

    public string GenerateToken(Account account)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("account_id", account.Id.ToString()),
                new Claim("account_username", account.Username),
                new Claim("role", "user")
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(_jwtSecretKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
