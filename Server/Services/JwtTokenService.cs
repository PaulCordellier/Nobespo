using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Server.Models;

namespace Server.Services;

public sealed class JwtTokenService(SecurityKey jwtSecretKey, IConfiguration configuration)
{
    private readonly SecurityKey _jwtSecretKey = jwtSecretKey;
    private readonly IConfiguration _configuration = configuration;

    // Model for this : https://github.com/adityaoberai/JWTAuthSample/blob/main/JWTAuth/Business/AuthService/Implementation/AuthService.cs

    public string GenerateToken(Account account)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("user_id", account.Id.ToString()),
                new Claim("username", account.Username),
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
