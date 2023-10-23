using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.App.Queries.Databases;
using MinimalApi.Dom.Enumerations;

namespace MinimalApi.Endpoints;

public class GetDatabaseEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/database");

        var namesGroup = group.MapGroup("/names");

        namesGroup.MapPost("/",
            GetDatabaseNameAsync);

        namesGroup.MapPost("/corporate",
            GetCorporateDatabaseNameAsync);

        namesGroup.MapPost("/corporate-rdb",
            GetCorporateRdbDatabaseNameAsync);

        namesGroup.MapPost("/parent",
            GetParentDatabaseNameAsync);
    }

    private static async Task<IResult> GetCorporateDatabaseNameAsync([FromServices] ISender mediator)
    {
        var query = new GetCorporateQuery(DatabaseSchemaTypes.Ipfs);
        var result = await mediator.Send(query);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }

    private static async Task<IResult> GetCorporateRdbDatabaseNameAsync([FromServices] ISender mediator)
    {
        var query = new GetCorporateQuery(DatabaseSchemaTypes.Rdb);
        var result = await mediator.Send(query);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }

    private static async Task<IResult> GetDatabaseNameAsync([FromServices] ISender mediator, [FromBody] GetNameQuery queryRequest)
    {
        var result = await mediator.Send(queryRequest);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }

    private static async Task<IResult> GetParentDatabaseNameAsync([FromServices] ISender mediator, [FromBody] GetParentQuery queryRequest)
    {
        var result = await mediator.Send(queryRequest);

        return result.Match<IResult>(
            response => TypedResults.Ok(response),
            errors => TypedResults.ValidationProblem(errors.ToDictionary(e => e.Code, e => new string[] { e.Description })));
    }
}
