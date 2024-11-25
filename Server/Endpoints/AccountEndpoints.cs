namespace Server.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("login", () => { });

        app.MapPost("signup", () => { });
    }
}
