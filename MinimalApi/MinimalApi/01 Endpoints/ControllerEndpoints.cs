using Microsoft.AspNetCore.Mvc;
using MinimalApi.Core;
using MinimalApi.Shared;

namespace MinimalApi.Endpoints;

public static class ControllerEndpoints
{
    public static void UseControllerEndpoints(this RouteGroupBuilder parentGroup)
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
