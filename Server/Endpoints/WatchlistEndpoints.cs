using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System.Security.Claims;

namespace Server.Endpoints;

public static class WatchlistEndpoints
{
    public static void MapWatchlistEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("watchlist").WithTags("Watchlists");

        route.MapPost("add", AddWatchlist);
    }

    private static async Task<IResult> AddWatchlist(ClaimsPrincipal claimsPrincipal,
                                                   [FromBody] Watchlist watchlist,
                                                   ApiDbContext dbContext)
    {
        Claim? userIdClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "user_id");

        if (!int.TryParse(userIdClaim?.Value, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        // Sanitizing string here:
        watchlist.Description = watchlist.Description.Trim().Replace("'", "''");

        watchlist.UserId = userId;

        await dbContext.Watchlists.AddAsync(watchlist);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }

}
