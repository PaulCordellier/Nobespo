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

        route.MapPost("login", Login);

        route.MapPost("signup", SignUp);

        // Route to modify the account :

        //route.MapPost("modify", (ClaimsPrincipal user) =>
        //{
        //    Claim? usernameClaim = user.Claims.FirstOrDefault(claim => claim.Type == "account_username");
        //})
        //.RequireAuthorization("user");
    }
    private static async Task<IResult> Login([FromBody] AccountConnexionRequestData requestData, JwtTokenService jwtTokenService, ApiDbContext dbContext)
    {
        Account account = new Account() { Id = 0, Password = requestData.Password, Username = requestData.Username };

        return Results.Ok(new { token = jwtTokenService.GenerateToken(account) });
    }

    private static async Task<IResult> SignUp([FromBody] AccountConnexionRequestData requestData, JwtTokenService jwtTokenService, ApiDbContext dbContext)
    {
        Account account = new Account() { Id = 0, Password = requestData.Password, Username = requestData.Username };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        return Results.Ok(new { Token = jwtTokenService.GenerateToken(account), Account = account });
    }

    class AccountConnexionRequestData
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}
