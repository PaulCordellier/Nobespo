using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Server.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("account").WithTags("Account");

        route.MapPost("login", Login);

        route.MapPost("signup", SignUp);
    }

    private static async Task<IResult> Login([FromBody] AccountConnexionRequestData requestData,
                                             JwtTokenService jwtTokenService,
                                             ApiDbContext dbContext)
    {
        byte[] hashedPassword = PlainTextPasswordToHash(requestData.Password);

        Account? foundAccount = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Username == requestData.Username && x.HashedPassword == hashedPassword);

        if (foundAccount is null)
        {
            return Results.NotFound();
        }

        // TODO I don't thing the client needs the ID to work
        return Results.Ok(new { Token = jwtTokenService.GenerateToken(foundAccount), Account = new { foundAccount.Id, foundAccount.Username } });
    }

    private static async Task<IResult> SignUp([FromBody] AccountConnexionRequestData requestData,
                                              JwtTokenService jwtTokenService,
                                              ApiDbContext dbContext)
    {
        bool usernameAlreadyExists = await dbContext.Accounts.AnyAsync(x => x.Username == requestData.Username);

        if (usernameAlreadyExists)
        {
            return Results.Conflict("Der Benutzername wird bereits verwendet.");
        }

        byte[] hashedPassword =  PlainTextPasswordToHash(requestData.Password);

        Account newAccount = new Account() { Id = 0, HashedPassword = hashedPassword, Username = requestData.Username };

        await dbContext.Accounts.AddAsync(newAccount);
        await dbContext.SaveChangesAsync();

        return Results.Ok(new { Token = jwtTokenService.GenerateToken(newAccount), Account = new { newAccount.Id, newAccount.Username } });
    }

    class AccountConnexionRequestData
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }

    private static byte[] PlainTextPasswordToHash(string plainTextPassword)
    {
        byte[] tmpSource = Encoding.UTF8.GetBytes(plainTextPassword);
        return SHA256.HashData(tmpSource);  // I could also use SHA-384 or SHA-512 here
    }
}
