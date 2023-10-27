using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Api.Features.Databases;

public class GetNameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/database");

        var namesGroup = group.MapGroup("/names");

        namesGroup.MapPost("/",
            GetDatabaseNameAsync);
    }

    private static async Task<IResult> GetDatabaseNameAsync([FromServices] ISender mediator, [FromBody] GetNameQuery queryRequest)
    {
        var result = await mediator.Send(queryRequest);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }
}
