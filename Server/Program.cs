using Server.Services;
using Server.Endpoints;
using Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// On the Dockerfile, this app is launched with the with-docker argument, which indicates that the app
// should get the user secrets from docker secrets and not from dotnet secrets
bool appIsUsingDocker = args.Length >= 1 && args[0] == "with-docker";

// This code section aims to get all the application secrets from dotnet secrets or docker secrets //

string? tmdbApiKey = null;
string? tmdbApiReadToken = null;
string? jwtSecretToken = null;
string? dbPassword = null;

if (appIsUsingDocker)
{
    // Trying to access secret data through docker secrets //

    const string DockerSecretsPath = "/run/secrets/";
    const string ApiKeyDockerSecretPath = DockerSecretsPath + "TmdbApiKey";
    const string ApiReadTokenDockerSecretPath = DockerSecretsPath + "TmdbApiReadToken";
    const string JwtSecretKeySecretPath = DockerSecretsPath + "JwtSecretKey";
    const string DbPasswordSecretPath = DockerSecretsPath + "DbPassword";

    if (File.Exists(ApiKeyDockerSecretPath))
    {
        tmdbApiKey = File.ReadAllText(ApiKeyDockerSecretPath);
    }
    if (File.Exists(ApiReadTokenDockerSecretPath))
    {
        tmdbApiReadToken = File.ReadAllText(ApiReadTokenDockerSecretPath);
    }
    if (File.Exists(JwtSecretKeySecretPath))
    {
        jwtSecretToken = File.ReadAllText(JwtSecretKeySecretPath);
    }
    if (File.Exists(DbPasswordSecretPath))
    {
        dbPassword = File.ReadAllText(DbPasswordSecretPath);
    }
}
else
{
    // Trying to access secret data through dontet secrets (doesn't work in production) //

    tmdbApiKey = builder.Configuration["TmdbApiKey"];
    tmdbApiReadToken = builder.Configuration["TmdbApiReadToken"];
    jwtSecretToken = builder.Configuration["JwtSecretKey"];
    dbPassword = builder.Configuration["DbPassword"];
}


// Verify that the code accessed the secrets
if (tmdbApiKey is null || tmdbApiReadToken is null || jwtSecretToken is null || dbPassword is null)
{
    string exceptionMessage = "You need to setup this app's secrets to start this project. For more" +
                              " information please read README.md. Secrets missing :";

    exceptionMessage += tmdbApiKey is null ? " TmdbApiKey" : "";
    exceptionMessage += tmdbApiReadToken is null ? " TmdbApiReadToken" : "";
    exceptionMessage += jwtSecretToken is null ? " JwtSecretKey" : "";
    exceptionMessage += dbPassword is null ? " DbPassword" : "";

    if (appIsUsingDocker)
    {
        exceptionMessage += ". The argument with-docker is detected when the app is launched so" +
            "the app is trying to access the secrets through docker secrets.";
    }
    else
    {
        exceptionMessage += ". The argument with-docker isn't detected when the app is launched" +
            "so the app is trying to access the secrets through dotnet secrets.";
    }

    throw new Exception(exceptionMessage);
}

SecurityKey jwtSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretToken));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddSingleton(x => new TmdbApiService(tmdbApiKey, tmdbApiReadToken));
builder.Services.AddSingleton(x => new JwtTokenService(jwtSecurityKey, builder.Configuration));

builder.Services.AddDbContext<ApiDbContext>(options =>
{
    string serverName = appIsUsingDocker ? "database" : "localhost";
    options.UseNpgsql($"Server={serverName};port=5432;Database=nobespo;uid=postgres;password={dbPassword};");
});

// Authentication and authorization docs: https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer
// Sample example: https://github.com/dotnet/aspnetcore/tree/main/src/Security/Authentication/JwtBearer/samples/MinimalJwtBearerSample

//Authentification handling
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = jwtSecurityKey
        };

        options.RequireHttpsMetadata = false;   // Set to true in production
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseDefaultFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTheme(ScalarTheme.Purple)
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch);
    });
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapMediaEndpoints();
app.MapCommentEndpoints();
app.MapAccountEndpoints();

app.Run();
