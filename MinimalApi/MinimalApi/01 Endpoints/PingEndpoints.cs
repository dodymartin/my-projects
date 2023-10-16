using Carter;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.App;
using MinimalApi.Shared;

namespace MinimalApi.Endpoints;

public class PingEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/ping");

        group.MapGet("/",
            PingAsync);

        group.MapPost("/uris",
            GetPingUrisAsync);
    }

    public static async Task<IResult> PingAsync()
    {
        return TypedResults.Ok(await Task.FromResult("Ping Successful!"));
    }

    private static async Task<IResult> GetPingUrisAsync([FromServices] PingService pingService, [FromBody, Validate] PingUriRequest request)
    {
        return TypedResults.Ok(await pingService.GetPingUrisAsync(request));
    }
}
