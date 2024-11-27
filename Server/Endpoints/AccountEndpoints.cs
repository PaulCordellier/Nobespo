using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Server.Services;
using Server.Models;

namespace Server.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("account").WithTags("Account");

        route.MapPost("login", ([FromBody] AccountConnexionRequestData requestData, JwtTokens jwtTokens) =>
        {
            Account account = new Account() { Id = 0, Password = requestData.Password, Username = requestData.Username };

            return Results.Ok(new { token = jwtTokens.GenerateToken(account) });
        });

        route.MapPost("signup", ([FromBody] AccountConnexionRequestData requestData, JwtTokens jwtTokens) =>
        {
            Account account = new Account() { Id = 0, Password = requestData.Password, Username = requestData.Username };

            return Results.Ok(new { Token = jwtTokens.GenerateToken(account), Account = account });
        });

        // Route to modify the account :

        //route.MapPost("modify", (ClaimsPrincipal user) =>
        //{
        //    Claim? usernameClaim = user.Claims.FirstOrDefault(claim => claim.Type == "account_username");
        //})
        //.RequireAuthorization("user");
    }

    class AccountConnexionRequestData
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}
