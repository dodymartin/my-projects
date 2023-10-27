using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Api.Features.Databases;

public class GetParentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/database");

        var namesGroup = group.MapGroup("/names");

        namesGroup.MapPost("/parent",
            GetParentDatabaseNameAsync);
    }

    private static async Task<IResult> GetParentDatabaseNameAsync([FromServices] ISender mediator, [FromBody] GetParentQuery queryRequest)
    {
        var result = await mediator.Send(queryRequest);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }
}
