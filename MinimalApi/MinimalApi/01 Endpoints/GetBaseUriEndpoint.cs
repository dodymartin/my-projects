using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.App.Queries.WebApis;

namespace MinimalApi.Endpoints;

public class GetBaseUriEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/base-uri");

        group.MapPost("/",
            GetBaseUriAsync);
    }

    private static async Task<IResult> GetBaseUriAsync([FromServices] ISender mediator, [FromBody] GetBaseUriQuery queryRequest)
    {
        var result = await mediator.Send(queryRequest);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }
}
