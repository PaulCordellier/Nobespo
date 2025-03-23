using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services;
using System.Security.Claims;

namespace Server.Endpoints;

public static class RatingEndpoints
{

    public static void MapRatingEndpoints(this IEndpointRouteBuilder app)
    {
        var baseRoute = app.MapGroup("rate").WithTags("Rate");

        baseRoute.RequireAuthorization();

        baseRoute.MapPost("film/{tmdbId}", RateFilm);
        baseRoute.MapPost("serie/{tmdbId}", RateSerie);
        baseRoute.MapPost("watchlist/{watchlistId}", RateWatchlist);

        baseRoute.MapGet("film/{tmdbId}", GetRatingFilm);
        baseRoute.MapGet("serie/{tmdbId}", GetRatingSerie);
        baseRoute.MapGet("watchlist/{watchlistId}", GetRatingWatchlist);
    }

    private static async Task<IResult> RateFilm(ClaimsPrincipal claimsPrincipal,
                                                [FromBody] int rating,
                                                ApiDbContext dbContext,
                                                int tmdbId,
                                                JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        if (rating is > 10 or < 1)
        {
            return Results.BadRequest("Bad rating");
        }

        FilmRating? filmRating = await dbContext.FilmRating.FirstOrDefaultAsync(x => x.UserId == userId && x.FilmId == tmdbId);

        if (filmRating is null)
        {
            filmRating = new FilmRating()
            {
                UserId = userId,
                FilmId = tmdbId,
                Rating = rating
            };

            dbContext.FilmRating.Add(filmRating);
        }
        else
        {
            filmRating.Rating = rating;
        }

        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> RateSerie(ClaimsPrincipal claimsPrincipal,
                                                 [FromBody] int rating,
                                                 ApiDbContext dbContext,
                                                 int tmdbId,
                                                 JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        if (rating is > 10 or < 1)
        {
            return Results.BadRequest("Bad rating");
        }

        SerieRating? serieRating = await dbContext.SerieRating.FirstOrDefaultAsync(x => x.UserId == userId && x.SerieId == tmdbId);

        if (serieRating is null)
        {
            serieRating = new SerieRating()
            {
                UserId = userId,
                SerieId = tmdbId,
                Rating = rating
            };

            dbContext.SerieRating.Add(serieRating);
        }
        else
        {
            serieRating.Rating = rating;
        }

        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> RateWatchlist(ClaimsPrincipal claimsPrincipal,
                                                     [FromBody] int rating,
                                                     ApiDbContext dbContext,
                                                     int watchlistId,
                                                     JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        if (rating is > 10 or < 1)
        {
            return Results.BadRequest("Bad rating");
        }

        WatchlistRating? watchlistRating = await dbContext.WatchlistRating.FirstOrDefaultAsync(x => x.UserId == userId && x.WatchlistId == watchlistId);

        if (watchlistRating is null)
        {
            watchlistRating = new WatchlistRating()
            {
                UserId = userId,
                WatchlistId = watchlistId,
                Rating = rating
            };

            dbContext.WatchlistRating.Add(watchlistRating);
        }
        else
        {
            watchlistRating.Rating = rating;
        }

        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> GetRatingFilm(ClaimsPrincipal claimsPrincipal,
                                                     ApiDbContext dbContext,
                                                     int tmdbId,
                                                     JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        FilmRating? filmRating = await dbContext.FilmRating.FirstOrDefaultAsync(x => x.UserId == userId && x.FilmId == tmdbId);

        if (filmRating is null)
        {
            return Results.Ok(0);
        }

        return Results.Ok(filmRating.Rating);
    }

    private static async Task<IResult> GetRatingSerie(ClaimsPrincipal claimsPrincipal,
                                                      ApiDbContext dbContext,
                                                      int tmdbId,
                                                      JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        SerieRating? serieRating = await dbContext.SerieRating.FirstOrDefaultAsync(x => x.UserId == userId && x.SerieId == tmdbId);

        if (serieRating is null)
        {
            return Results.Ok(0);
        }

        return Results.Ok(serieRating.Rating);
    }

    private static async Task<IResult> GetRatingWatchlist(ClaimsPrincipal claimsPrincipal,
                                                          ApiDbContext dbContext,
                                                          int watchlistId,
                                                          JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        WatchlistRating? watchlistRating = await dbContext.WatchlistRating.FirstOrDefaultAsync(x => x.UserId == userId && x.WatchlistId == watchlistId);

        if (watchlistRating is null)
        {
            return Results.Ok(0);
        }

        return Results.Ok(watchlistRating.Rating);
    }
}
