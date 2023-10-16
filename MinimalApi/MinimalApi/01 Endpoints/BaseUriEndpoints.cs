using Carter;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.App;
using MinimalApi.Shared;

namespace MinimalApi.Endpoints;

public class BaseUriEndpoints :ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
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
