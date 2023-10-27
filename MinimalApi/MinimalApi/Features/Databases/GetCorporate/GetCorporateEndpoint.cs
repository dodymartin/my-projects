using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Databases;

public class GetCorporateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/database");

        var namesGroup = group.MapGroup("/names");

        namesGroup.MapPost("/corporate",
            GetCorporateDatabaseNameAsync);

        namesGroup.MapPost("/corporate-rdb",
            GetCorporateRdbDatabaseNameAsync);
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
}
