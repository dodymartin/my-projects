using Carter;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.App;
using MinimalApi.Shared;

namespace MinimalApi.Endpoints;

public class ControllerEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/controller");

        group.MapPost("/uris",
            GetControllerUrisAsync);
    }

    private static async Task<IResult> GetControllerUrisAsync([FromServices] ControllerService controllerService, [FromBody, Validate] ControllerUriRequest request)
    {
        return TypedResults.Ok(await controllerService.GetControllerUriInfoAsync(request));
    }
}
