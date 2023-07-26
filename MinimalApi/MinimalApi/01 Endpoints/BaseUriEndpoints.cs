using Microsoft.AspNetCore.Mvc;
using MinimalApi.Core;
using MinimalApi.Shared;

namespace MinimalApi.Endpoints;

public static class BaseUriEndpoints
{
    public static void UseBaseUriEndpoints(this RouteGroupBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/base-uri");

        group.MapPost("/",
            GetBaseUriAsync);
    }

    private static async Task<IResult> GetBaseUriAsync([FromServices] BaseUriService baseUriService, [FromBody, Validate] BaseUriRequest request)
    {
        return TypedResults.Ok(await baseUriService.GetBaseUriAsync(request));
    }
}
