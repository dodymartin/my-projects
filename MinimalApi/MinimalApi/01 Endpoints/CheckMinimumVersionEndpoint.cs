using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.App.Queries.Applications;

namespace MinimalApi.Endpoints;

public class CheckMinimumVersionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/version");

        group.MapPost("/check-minimum",
            CheckMinimumVersionAsync);
    }

    private static async Task<IResult> CheckMinimumVersionAsync([FromServices] ISender mediator, [FromBody] CheckMinimumVersionQuery queryRequest)
    {
        var result = await mediator.Send(queryRequest);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }
}
