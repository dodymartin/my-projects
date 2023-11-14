using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Api.Features.WebApis;

public sealed class GetPingUrisEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/ping");

        group.MapPost("/uris",
            GetPingUrisAsync);
    }

    private static async Task<IResult> GetPingUrisAsync([FromServices] ISender mediator, [FromBody] GetPingsQuery query)
    {
        var result = await mediator.Send(query);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }
}
