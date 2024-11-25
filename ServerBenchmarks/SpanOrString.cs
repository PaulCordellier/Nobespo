using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using Server.Services;
using System.Net.Http;


/// <summary>
/// In the class MediaEndpoints, the response from the API TMDB is redirected directly.
/// This benchmark is just used to know which method is the best to do that.
/// </summary>
[MediumRunJob, MemoryDiagnoser]
public class SpanOrString
{
    private HttpContext _httpContext = null!;
    private HttpResponseMessage _httpResponseMessage = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        _httpContext = new DefaultHttpContext();
        _httpResponseMessage = await new TmdbApi().GetTrending();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _httpContext = new DefaultHttpContext();
    }

    /// <summary>
    /// Creates the response of an HttpContext object form the content of an HttpResponseMessage.
    /// </summary>
    [Benchmark]
    public async Task CreateContextFromResponseMessageWithSpan()
    {
        if (_httpResponseMessage.IsSuccessStatusCode)
        {
            _httpContext.Response.ContentType = "application/json";

            await _httpResponseMessage.Content.CopyToAsync(_httpContext.Response.Body);
        }
        else
        {
            _httpContext.Response.StatusCode = (int)_httpResponseMessage.StatusCode;
        }
    }

    [Benchmark]
    public async Task CreateContextFromResponseMessageWithString()
    {
        if (_httpResponseMessage.IsSuccessStatusCode)
        {
            _httpContext.Response.ContentType = "application/json";

            string contentAsString = await _httpResponseMessage.Content.ReadAsStringAsync();
            await _httpContext.Response.WriteAsync(contentAsString);
        }
        else
        {
            _httpContext.Response.StatusCode = (int)_httpResponseMessage.StatusCode;
        }
    }
}

// Results : 
// | Method                                      | Mean         | Error      | StdDev     | Gen0    | Allocated  |
// | ------------------------------------------- | ------------:| ----------:| ----------:| -------:| ----------:|
// | CreateContextFromResponseMessageWithSpan    | 35.99 ns     | 0.271 ns   | 0.379 ns   | -       | -          |
// | CreateContextFromResponseMessageWithString  | 5,222.67 ns  | 23.117 ns  | 33.885 ns  | 5.8441  | 24504 B    |


// Using the span is better!