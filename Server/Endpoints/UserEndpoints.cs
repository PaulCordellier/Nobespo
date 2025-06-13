using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace Server.Endpoints;

public static partial class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("user").WithTags("User");

        route.MapGet("searchbyusername/{username}", SearchByUsername);

        route.MapPost("login", Login);
        route.MapPost("signup", SignUp);

        route.MapPost("follow/{username}", FollowUser);
        route.MapPost("unfollow/{username}", UnfollowUser);
    }

    private static async Task<IResult> SearchByUsername(string username, ClaimsPrincipal claimsPrincipal, ApiDbContext dbContext, JwtTokenService tokenService)
    {
        User? user;

        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId) ||
            (user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId)) is null)
        {
            return Results.BadRequest("Bad token");
        }

        var foundUsernames = await dbContext.SearchUsernames(username)
                                            .Select(x => new { x.Username, FollowsCurrentUser = x.Followers.Contains(user) })
                                            .ToArrayAsync();

        return Results.Ok(foundUsernames);
    }

    private static async Task<IResult> Login([FromBody] UserConnexionRequestData requestData,
                                             JwtTokenService jwtTokenService,
                                             ApiDbContext dbContext)
    {
        if (!UsernameIsValid(requestData.Username))
        {
            // We return 404 Not Found because the login can't be found in the database
            return Results.NotFound("Bad login format");
        }

        byte[] hashedPassword = PlainTextPasswordToHash(requestData.Password);
            
        User? foundUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == requestData.Username && x.HashedPassword == hashedPassword);

        if (foundUser is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new { Token = jwtTokenService.GenerateToken(foundUser), foundUser.Username });
    }

    private static async Task<IResult> SignUp([FromBody] UserConnexionRequestData requestData,
                                              JwtTokenService jwtTokenService,
                                              ApiDbContext dbContext)
    {
        if (!UsernameIsValid(requestData.Username))
        {
            return Results.BadRequest("Bad login format");
        }

        bool usernameAlreadyExists = await dbContext.Users.AnyAsync(x => x.Username == requestData.Username);

        if (usernameAlreadyExists)
        {
            return Results.Conflict("Der Benutzername wird bereits verwendet.");
        }

        byte[] hashedPassword =  PlainTextPasswordToHash(requestData.Password);

        User newUser = new() { HashedPassword = hashedPassword, Username = requestData.Username };

        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();

        return Results.Ok(new { Token = jwtTokenService.GenerateToken(newUser), newUser.Username });
    }

    class UserConnexionRequestData
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }

    private static byte[] PlainTextPasswordToHash(string plainTextPassword)
    {
        byte[] tmpSource = Encoding.UTF8.GetBytes(plainTextPassword);
        return SHA256.HashData(tmpSource);
    }

    [GeneratedRegex("^[a-z0-9_-]*$")]
    private static partial Regex UsernameRegex();

    private static bool UsernameIsValid(in string username)
    {
        return username.Length <= 25 && UsernameRegex().Match(username).Success;
    }

    private async static Task<IResult> FollowUser(string username, ClaimsPrincipal claimsPrincipal, ApiDbContext dbContext, JwtTokenService tokenService)
    {
        User? currentUser;

        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId) ||
            (currentUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId)) is null)
        {   
            return Results.BadRequest("Bad token");
        }

        User? userToFollow = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

        if (userToFollow is null)
        {
            return Results.NotFound();
        }

        currentUser.UsersFollowed.Add(userToFollow);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }

    private async static Task<IResult> UnfollowUser(string username, ClaimsPrincipal claimsPrincipal, ApiDbContext dbContext, JwtTokenService tokenService)
    {
        User? currentUser;

        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId) ||
            (currentUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId)) is null)
        {
            return Results.BadRequest("Bad token");
        }

        User? userToUnfollow = await dbContext.Users
                                              .Include(x => x.Followers)
                                              .FirstOrDefaultAsync(x => x.Username == username && x.Followers.Contains(currentUser));

        if (userToUnfollow is null)
        {
            return Results.NotFound();
        }

        currentUser.UsersFollowed.Remove(userToUnfollow);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }
}
