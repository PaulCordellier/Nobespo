using Server.Services;
using Server.Endpoints;
using Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);


// This code section aims to get all the application secrets from dotnet secrets or docker secrets //

// Trying to access secret data through dontet secrets (doesn't work in production) //

string? tmdbApiKey = builder.Configuration["TmdbApi:Key"];
string? tmdbApiReadToken = builder.Configuration["TmdbApi:ReadToken"];
string? jwtSecretToken = builder.Configuration["Jwt:SecretKey"];


// Trying to access secret data through docker secrets only works in container //

const string DockerSecretsPath = "/run/secrets/";
const string ApiKeyDockerSecretPath = DockerSecretsPath + "tmdbapikey";
const string ApiReadTokenDockerSecretPath = DockerSecretsPath + "tmdbapireadtoken";
const string JwtSecretKeySecretPath = DockerSecretsPath + "jwtsecretkey";

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


// Verify that the code accessed the secrets
if (tmdbApiKey is null || tmdbApiReadToken is null || jwtSecretToken is null)
{
    string exceptionMessage = "You need to setup this app's secrets to start this project. For more information please read README.md. Secrets missing :";
    exceptionMessage += tmdbApiKey is null ? " TmdbApi:Key" : "";
    exceptionMessage += tmdbApiReadToken is null ? " TmdbApi:ReadToken" : "";
    exceptionMessage += jwtSecretToken is null ? " Jwt:SecretKey" : "";
    exceptionMessage += ".";

    throw new Exception(exceptionMessage);
}

SecurityKey jwtSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretToken));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSingleton(x => new TmdbApi(tmdbApiKey, tmdbApiReadToken));
builder.Services.AddSingleton(x => new JwtTokens(jwtSecurityKey, builder.Configuration));
builder.Services.AddDbContext<ApiDbContext>(options => options.UseNpgsql("TODO"));

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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("user", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("role", "user");
    });

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

app.MapFallbackToFile("/index.html");

app.Run();
