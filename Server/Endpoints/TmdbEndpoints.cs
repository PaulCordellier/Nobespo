using Server.Services;

namespace Server.Endpoints;

// For this project I am using the TMDB API (docs: https://developer.themoviedb.org/reference/intro/getting-started)
public static class TmdbEndpoints
{
    public static void MapMediaEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("tmdb").WithTags("TMDB");

        route.MapGet("/search-films-and-series/{query}", async (string query, HttpContext context, TmdbApi tmdbApi) =>
        {
            HttpResponseMessage response = await tmdbApi.SearchMulti(query);
            await CreateContextFromResponseMessage(context, response);
        });

        route.MapGet("/trending", async (HttpContext context, TmdbApi tmdbApi, HttpRequest request) =>
        {
            HttpResponseMessage response = await tmdbApi.GetTrending();
            await CreateContextFromResponseMessage(context, response);
        });

        route.MapGet("/movie/{id:int}", async (int id, HttpContext context, TmdbApi tmdbApi) =>
        {
            HttpResponseMessage response = await tmdbApi.GetMovieDetail(id);
            await CreateContextFromResponseMessage(context, response);
        });

        route.MapGet("/serie/{id:int}", async (int id, HttpContext context, TmdbApi tmdbApi) =>
        {
            HttpResponseMessage response = await tmdbApi.GetSerieDetail(id);
            await CreateContextFromResponseMessage(context, response);
        });
    }

    /// <summary>
    /// Creates the response of an HttpContext object form the content of an HttpResponseMessage.
    /// </summary>
    private async static Task CreateContextFromResponseMessage(HttpContext context, HttpResponseMessage responseMessage)
    {
        if (responseMessage.IsSuccessStatusCode)
        {
            context.Response.ContentType = "application/json";
            await responseMessage.Content.CopyToAsync(context.Response.Body);
        }
        else
        {
            context.Response.StatusCode = (int)responseMessage.StatusCode;
        }
    }
}
