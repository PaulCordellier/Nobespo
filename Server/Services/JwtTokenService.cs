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

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("user_id", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("role", "user")
            ]),
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials = new SigningCredentials(_jwtSecretKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Attempts to extract a user's ID from a ClaimsPrincipal object. If the extraction
    /// succeeds, the method assigns the parsed value to the userId parameter and returns
    /// true. Otherwise, it returns false.
    /// </summary>
    public bool TryGetUserIdFromClaims(ClaimsPrincipal claimsPrincipal, out int userId)
    {
        Claim? userIdClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "user_id");

        return int.TryParse(userIdClaim?.Value, out userId);
    }
}
