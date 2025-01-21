using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services;
using System.Security.Claims;
using System.Text.Json;

namespace Server.Endpoints;

public static class WatchlistEndpoints
{
    public static void MapWatchlistEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("watchlist").WithTags("Watchlists");

        route.MapGet("getbyid/{watchlistId}", GetById);
        route.MapGet("recent", GetRecentWatchlists);
        route.MapGet("search/{encodedSearchText}", SearchWatchlists);
        route.MapPost("add", AddWatchlist);
    }

    private static async Task<IResult> GetById(int watchlistId, ApiDbContext dbContext)
    {
        Watchlist? foundWatchlist = await dbContext.Watchlists.FirstOrDefaultAsync(x => x.Id == watchlistId);

        if (foundWatchlist is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(foundWatchlist);
    }

    private static async Task<IResult> GetRecentWatchlists(ApiDbContext dbContext)
    {
        var recentWatchlists = await dbContext.Watchlists
                                              .OrderBy(x => x.PublishDate)
                                              .Take(10)
                                              .Select(x => new { x.Id, x.Title, x.Description, x.FilmsIds, x.SeriesIds, x.PublishDate, x.User.Username, x.PosterPaths })
                                              .ToArrayAsync();

        return Results.Ok(recentWatchlists);
    }

    private static async Task<IResult> SearchWatchlists(string encodedSearchText, ApiDbContext dbContext)
    {
        string decodedSearchText = Uri.UnescapeDataString(encodedSearchText);

        Watchlist[] foundWatchlists = await dbContext.SearchWatchlists(decodedSearchText).ToArrayAsync();

        return Results.Ok(foundWatchlists);
    }

    private static async Task<IResult> AddWatchlist(ClaimsPrincipal claimsPrincipal,
                                                   [FromBody] Watchlist watchlist,
                                                   ApiDbContext dbContext,
                                                   TmdbApiService tmdbApi,
                                                   JwtTokenService tokenService)
    {
        if (!tokenService.TryGetUserIdFromClaims(claimsPrincipal, out int userId))
        {
            return Results.BadRequest("Bad token");
        }

        if (watchlist.FilmsIds.Count + watchlist.SeriesIds.Count < 2)
        {
            return Results.BadRequest("A watchlist needs at least two films or series.");
        }

        watchlist.UserId = userId;

        // Here, we store the poster paths of some films and series of the watchlist so we can show some 
        // posters to the frontend. This is useful on pages where there is a lot of whatchlists, the user
        // can have a quick preview of the films in the whatchlist by just looking at the posters.
            
        watchlist.PosterPaths.Clear();

        int index = 0;
        ICollection<int> mediaIdCollection = watchlist.FilmsIds;

        while (watchlist.PosterPaths.Count < 4)
        {
            if (mediaIdCollection.Count <= index && mediaIdCollection == watchlist.FilmsIds)
            {
                mediaIdCollection = watchlist.SeriesIds;
                index = 0;
                continue;
            }
            else if (mediaIdCollection.Count <= index)
            {
                break;
            }

            string? foundPoster = await GetPosterPath(id: mediaIdCollection.ElementAt(index),
                                                      mediaIsFilm: mediaIdCollection == watchlist.FilmsIds,
                                                      tmdbApi);

            if (foundPoster is not null)
            {
                watchlist.PosterPaths.Add(foundPoster);
            }

            index++;
        }

        await dbContext.Watchlists.AddAsync(watchlist);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }

    private async static Task<string?> GetPosterPath(int id, bool mediaIsFilm, TmdbApiService tmdbApi)
    {
        HttpResponseMessage responseMessage;

        if (mediaIsFilm)
        {
            responseMessage = await tmdbApi.GetMovieDetail(id);
        }
        else
        {
            responseMessage = await tmdbApi.GetSerieDetail(id);
        }

        Dictionary<string, JsonElement>? jsonDict = await responseMessage.Content.ReadFromJsonAsync<Dictionary<string, JsonElement>>();

        const string PosterPathKeyName = "poster_path";

        if (jsonDict is null
            || !jsonDict.TryGetValue(PosterPathKeyName, out JsonElement posterPath)
            || posterPath.ValueKind != JsonValueKind.String)
        {
            return null;
        }

        return posterPath.ToString();
    }
}
