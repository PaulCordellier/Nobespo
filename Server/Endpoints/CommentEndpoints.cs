namespace Server.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("comment").WithTags("Comment");

        route.MapPost("", () => { });
    }
}
