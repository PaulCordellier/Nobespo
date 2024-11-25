using System.Net.Http.Headers;
using System.Text.Json;
using Server.Models;

namespace Server.Services;

public sealed class TmdbApi
{
    private HttpClient _httpClient;     // An HttpClient should be created only once

    public TmdbApi(string apiKey, string apiReadToken)
    {
        _httpClient = new HttpClient();
        Console.WriteLine("HttpClient created !");  // TODO asupp
        _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiReadToken);
    }

    // Docs : https://developer.themoviedb.org/reference/search-multi
    public async Task<HttpResponseMessage> SearchMulti(string query, string language = "en-US")
    {
        return await _httpClient.GetAsync($"search/multi?query={query}&language={language}");
    }
    
    // Docs : https://developer.themoviedb.org/reference/trending-all
    public async Task<HttpResponseMessage> GetTrending(string language = "en-US")
    {
        return await _httpClient.GetAsync($"trending/all/day?language={language}");
    }

    // Docs : https://developer.themoviedb.org/reference/movie-details
    public async Task<HttpResponseMessage> GetMovieDetail(int id, string language = "en-US")
    {
        return await _httpClient.GetAsync($"movie/{id}?language={language}");
    }

    // Docs : https://developer.themoviedb.org/reference/tv-series-details
    public async Task<HttpResponseMessage> GetSerieDetail(int id, string language = "en-US")
    {
        return await _httpClient.GetAsync($"tv/{id}?language={language}");
    }
}
