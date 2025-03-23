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

        publishCommentRoute.MapPost("film/{tmdbId}", CommentFilm);
        publishCommentRoute.MapPost("serie/{tmdbId}", CommentSerie);
        publishCommentRoute.MapPost("watchlist/{watchlistId}", CommentWatchlist);

        var getCommentRoute = baseRoute.MapGroup("get");

        getCommentRoute.MapGet("film/{tmdbId}", GetFilmComment);
        getCommentRoute.MapGet("serie/{tmdbId}", GetSerieComment);
        getCommentRoute.MapGet("watchlist/{watchlistId}", GetWatchlistComment);
    }

    private static async Task<IResult> GetFilmComment(ApiDbContext dbContext, int tmdbId)
    {
        var comments = await dbContext.FilmsComments
                                      .Where(x => x.TmdbFilmId == tmdbId)
                                      .OrderBy(x => x.PublishDate)
                                      .Take(10)
                                      .Select(x => new { x.Text, x.PublishDate, x.User.Username, x.TmdbFilmId, x.UserId })
                                      .ToArrayAsync();

        var commentsWithRating = comments.Select(x => new { x.Text, x.PublishDate, x.Username, Rating = GetRating(x.TmdbFilmId, x.UserId) })
                                         .ToArray();

        int GetRating(int watchlistId, int userId)
        {
            FilmRating? rating = dbContext.FilmRating
                                          .FirstOrDefault(x => x.FilmId == watchlistId && x.UserId == userId);

            if (rating is null)
            {
                return 0;
            }

            return rating.Rating;
        }

        return Results.Ok(commentsWithRating);
    }

    private static async Task<IResult> GetSerieComment(ApiDbContext dbContext, int tmdbId)
    {
        var comments = await dbContext.SeriesComments
                                      .Where(x => x.TmdbSerieId == tmdbId)
                                      .OrderBy(x => x.PublishDate)
                                      .Take(10)
                                      .Select(x => new { x.Text, x.PublishDate, x.User.Username, x.TmdbSerieId, x.UserId })
                                      .ToArrayAsync();

        var commentsWithRating = comments.Select(x => new { x.Text, x.PublishDate, x.Username, Rating = GetRating(x.TmdbSerieId, x.UserId) })
                                         .ToArray();

        int GetRating(int watchlistId, int userId)
        {
            SerieRating? rating = dbContext.SerieRating
                                           .FirstOrDefault(x => x.SerieId == watchlistId && x.UserId == userId);

            if (rating is null)
            {
                return 0;
            }

            return rating.Rating;
        }

        return Results.Ok(commentsWithRating);
    }

    private static async Task<IResult> GetWatchlistComment(ApiDbContext dbContext, int watchlistId)
    {
        var comments = await dbContext.WatchlistComments
                                      .Where(x => x.WatchlistId == watchlistId)
                                      .OrderBy(x => x.PublishDate)
                                      .Take(10)
                                      .Select(x => new { x.Text, x.PublishDate, x.User.Username, x.WatchlistId, x.UserId })
                                      .ToArrayAsync();

        var commentsWithRating = comments.Select(x => new { x.Text, x.PublishDate, x.Username, Rating = GetRating(x.WatchlistId, x.UserId) })
                                         .ToArray();

        int GetRating(int watchlistId, int userId)
        {
            WatchlistRating? rating = dbContext.WatchlistRating
                                               .FirstOrDefault(x => x.WatchlistId == watchlistId && x.UserId == userId);

            if (rating is null)
            {
                return 0;
            }

            return rating.Rating;
        }

        return Results.Ok(commentsWithRating);
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

        var commentToAdd = new FilmComment
        {
            Text = commentText,
            UserId = userId,
            TmdbFilmId = tmdbId
        };

        dbContext.FilmsComments.Add(commentToAdd);
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

        var commentToAdd = new SerieComment
        {
            Text = commentText,
            UserId = userId,
            TmdbSerieId = tmdbId
        };

        dbContext.SeriesComments.Add(commentToAdd);
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

        var watchlistToAdd = new WatchlistComment
        {
            Text = commentText,
            UserId = userId,
            WatchlistId = watchlistId
        };

        dbContext.WatchlistComments.Add(watchlistToAdd);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }
}
