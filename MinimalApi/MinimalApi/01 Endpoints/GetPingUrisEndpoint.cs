using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.App.Queries.WebApis;

namespace MinimalApi.Endpoints;

public class GetPingUrisEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/ping");

        group.MapGet("/",
            PingAsync);

        group.MapPost("/uris",
            GetPingUrisAsync);
    }

    public static async Task<IResult> PingAsync()
    {
        return TypedResults.Ok(await Task.FromResult("Ping Successful!"));
    }

    private static async Task<IResult> GetPingUrisAsync([FromServices] ISender mediator, [FromBody] GetPingUrisQuery query)
    {
        var result = await mediator.Send(query);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }
}
