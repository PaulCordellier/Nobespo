# Nobespo

Welcome to this project, a website in which you can create movies lists, rate, comment movies and series, similar to [Letterboxd](https://letterboxd.com/). It uses [The Movie Database](https://www.themoviedb.org) to get information the movies and the series.

## Start

Open the app at the root of the project with
```
docker-compose build
docker-compose up
```


## Secrets

To run the app, you need to generate passwords and get [API keys form The Movie Database](https://developer.themoviedb.org/docs/getting-started). If you start the app, with Docker, you will need to setup Docker secrets, if not, use .NET secrets.

### Docker secrets

You need to create the files
DbPassword.txt
JwtSecretKey.txt
TmdbApiKey.txt
TmdbApiReadToken.txt
in the secrets folder. Each file has it's corresponding secret. These secrets will be read with the docker-compose.yml file.

### .NET secrets

You need to setup the three secrets DbPassword, JwtSecretKey, TmdbApiKey and TmdbApiReadToken with their corresponding value. Here's a link [that shows you how to do that](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) just in case.


## Certificate

Docker is setup with https but you need to create your own certificates

For the server, you can use dotnet dev-certs to generate the certificates. Run these commands in the Server folder:
```
dotnet dev-certs https -ep .\ssl-certificate.pfx -p NobespoSslPassword
dotnet dev-certs https --trust
```

For the client, you can use openssl. Run this command in the client folder:
```
openssl req -x509 -sha256 -nodes -days 365 -newkey rsa:2048 -keyout ssl-private-key.key -out ssl-certificate.crt
```


## Migrations

The server API uses Entity Framework Core and Npgsql to handle the API with C# code. To generate the SQL scripts from C# code you need to:

Install dotnet-ef:
```
dotnet tool install --global dotnet-ef
```

Add a database migration:
```
dotnet ef migrations add NobespoDbMigrations --project Server
```

Get the generated SQL script form the C# code:
```
dotnet ef migrations script --project Server --output ./database/generated_creation_script.sql
```

To remove the migrations:
```
dotnet ef migrations remove --project Server
```
