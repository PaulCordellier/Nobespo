using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services;
using System.Security.Claims;

namespace Server.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this IEndpointRouteBuilder app)
    {
        var baseRoute = app.MapGroup("comment").WithTags("Comment");

        var publishCommentRoute = baseRoute.MapGroup("publish");

        publishCommentRoute.RequireAuthorization();

        publishCommentRoute.MapPost("film/{tmdbId}", CommentSerie);
        publishCommentRoute.MapPost("serie/{tmdbId}", CommentFilm);
        publishCommentRoute.MapPost("watchlist/{watchlistId}", CommentWatchlist);

        var getCommentRoute = baseRoute.MapGroup("get");

        getCommentRoute.MapGet("film/{tmdbId}", GetSerieComment);
        getCommentRoute.MapGet("serie/{tmdbId}", GetFilmComment);
        getCommentRoute.MapGet("watchlist/{watchlistId}", GetWatchlistComment);
    }

    private static async Task<IResult> GetFilmComment(ApiDbContext dbContext, int tmdbId)
    {
        var mostRecentComents = await dbContext.FilmsComments
                                               .Where(x => x.TmdbFilmId == tmdbId)
                                               .OrderBy(x => x.PublishDate)
                                               .Take(10)
                                               .Select(x => new { x.Text, x.PublishDate, x.User.Username })
                                               .ToArrayAsync();
        return Results.Ok(mostRecentComents);
    }

    private static async Task<IResult> GetSerieComment(ApiDbContext dbContext, int tmdbId)
    {
        var mostRecentComents = await dbContext.SeriesComments
                                               .Where(x => x.TmdbSerieId == tmdbId)
                                               .OrderBy(x => x.PublishDate)
                                               .Take(10)
                                               .Select(x => new { x.Text, x.PublishDate, x.User.Username })
                                               .ToArrayAsync();
        return Results.Ok(mostRecentComents);
    }

    private static async Task<IResult> GetWatchlistComment(ApiDbContext dbContext, int watchlistId)
    {
        var mostRecentComents = await dbContext.WatchlistComments
                                               .Where(x => x.WatchlistId == watchlistId)
                                               .OrderBy(x => x.PublishDate)
                                               .Take(10)
                                               .Select(x => new { x.Text, x.PublishDate, x.User.Username })
                                               .ToArrayAsync();
        return Results.Ok(mostRecentComents);
    }

    private static async Task<IResult> CommentFilm(ClaimsPrincipal claimsPrincipal,
                                                   [FromBody] string commentText,
                                                   ApiDbContext dbContext,
                                                   int tmdbId,
                                                   JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        // Sanitizing string here:
        commentText = commentText.Trim().Replace("'", "''");

        var commentToAdd = new FilmComment
        {
            Text = commentText,
            UserId = userId,
            TmdbFilmId = tmdbId
        };

        await dbContext.FilmsComments.AddAsync(commentToAdd);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> CommentSerie(ClaimsPrincipal claimsPrincipal,
                                                   [FromBody] string commentText,
                                                   ApiDbContext dbContext,
                                                   int tmdbId,
                                                   JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        // Sanitizing string here:
        commentText = commentText.Trim().Replace("'", "''");

        var commentToAdd = new SerieComment
        {
            Text = commentText,
            UserId = userId,
            TmdbSerieId = tmdbId
        };

        await dbContext.SeriesComments.AddAsync(commentToAdd);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> CommentWatchlist(ClaimsPrincipal claimsPrincipal,
                                                        [FromBody] string commentText,
                                                        ApiDbContext dbContext,
                                                        int watchlistId,
                                                        JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        // Sanitizing string here:
        commentText = commentText.Trim().Replace("'", "''");

        var watchlistToAdd = new WatchlistComment
        {
            Text = commentText,
            UserId = userId,
            WatchlistId = watchlistId
        };

        await dbContext.WatchlistComments.AddAsync(watchlistToAdd);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }
}
