using Server.Services;
using Server.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(x =>
{
    // This code aims to create a TmdbApi object with API keys from dotnet secrets or docker secrets //

    // Trying to access secret data through dontet secrets : (doesn't work in production) //

    string? tmdbApiKey = builder.Configuration["TmdbApi:Key"];
    string? tmdbApiReadToken = builder.Configuration["TmdbApi:ReadToken"];


    // Trying to access secret data through docker secrets only works in container //

    const string DockerSecretsPath = "/run/secrets/";
    const string ApiKeyDockerSecretPath = DockerSecretsPath + "tmdbapikey";
    const string ApiReadTokenDockerSecretPath = DockerSecretsPath + "tmdbapireadtoken";

    if (File.Exists(ApiKeyDockerSecretPath))
    {
        tmdbApiKey = File.ReadAllText(ApiKeyDockerSecretPath);
    }

    if (File.Exists(ApiReadTokenDockerSecretPath))
    {
        tmdbApiReadToken = File.ReadAllText(ApiReadTokenDockerSecretPath);
    }


    // Verify that the code accessed the secrets
    if (tmdbApiKey is null || tmdbApiReadToken is null)
    {
        string exceptionMessage = "You need TMDB API keys to start this project. For more information please read README.md.";
        exceptionMessage += tmdbApiKey is null ? " The secret TmdbApi:Key is missing." : "";
        exceptionMessage += tmdbApiReadToken is null ? " The secret TmdbApi:ReadToken is missing." : "";

        throw new Exception(exceptionMessage);
    }

    return new TmdbApi(tmdbApiKey, tmdbApiReadToken); 
});

// Authentication and authorization docs: https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer
// Sample example: https://github.com/dotnet/aspnetcore/tree/main/src/Security/Authentication/JwtBearer/samples/MinimalJwtBearerSample

//Authentification handling
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = "your_issuer",
//            ValidAudience = "your_audience",
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"))
//        };
//    });

var app = builder.Build();

app.UseDefaultFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapMediaEndpoints();
app.MapCommentEndpoints();
app.MapAccountEndpoints();

app.MapFallbackToFile("/index.html");

app.Run();
