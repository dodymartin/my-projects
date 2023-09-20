using Carter;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Core;
using MinimalApi.Shared;

namespace MinimalApi.Endpoints;

public class VersionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/version");

        group.MapPost("/check-minimum",
            CheckMinimumVersionAsync);
    }

    private static async Task<IResult> CheckMinimumVersionAsync([FromServices] VersionService versionService, [FromBody, Validate] VersionCheckMinimumRequest request)
    {
        return TypedResults.Ok(await versionService.CheckMinimumVersionAsync(request));
    }
}
