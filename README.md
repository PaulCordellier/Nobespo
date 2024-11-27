# Nobespo

Welcome to this project, a website in which you can create movies lists, rate, comment movies and series, similar to [Letterboxd](https://letterboxd.com/). It uses [The Movie Database](https://www.themoviedb.org) to get information the movies and the series.

## Secrets

To run the app, you need to generate passwords and get [API keys form The Movie Database](https://developer.themoviedb.org/docs/getting-started). If you start the app, with Docker, you will need to setup Docker Secrets, if not, user Dotnet Secrets.

## Migrations

The server API uses Entity Framework Core and Npgsql to handle the API with C# code. To generate the SQL scripts from C# code you need to:

Install dotnet-ef:
```
dotnet tool install --global dotnet-ef
```

Add a database migration:
```
dotnet-ef migrations add NobespoDbMigrations --project Server
```

Get the generated SQL script form the C# code:
```
dotnet-ef migrations script --project Server --output ./database/generated_creation_script.sql
```