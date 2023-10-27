using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Api.Features.WebApis;

public class GetControllerUrisEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/controller");

        group.MapPost("/uris",
            GetControllerUrisAsync);
    }

    private static async Task<IResult> GetControllerUrisAsync([FromServices] ISender mediator, [FromBody] GetControllerUrisQuery queryRequest)
    {
        var result = await mediator.Send(queryRequest);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }
}
