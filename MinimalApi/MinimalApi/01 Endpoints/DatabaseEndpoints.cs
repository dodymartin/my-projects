using Microsoft.AspNetCore.Mvc;
using MinimalApi.Dal;
using MinimalApi.Services;

namespace MinimalApi.Endpoints;

public static class DatabaseEndpoints
{
    public static void UseDatabaseEndpoints(this RouteGroupBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/database");

        var namesGroup = group.MapGroup("/names");

        group.MapPost("/",
            GetDatabaseNameAsync);

        group.MapPost("/corporate",
            GetCorporateDatabaseNameAsync);

        group.MapPost("/corporate/rdb",
            GetCorporateRdbDatabaseNameAsync);

        group.MapPost("/parent",
            GetParentDatabaseNameAsync);
    }

    private static async Task<IResult> GetCorporateDatabaseNameAsync([FromServices] DatabaseService databaseService)
    {
        return TypedResults.Ok(await databaseService.GetCorporateDatabaseNameAsync(DatabaseSchemaTypes.Ipfs));
    }

    private static async Task<IResult> GetCorporateRdbDatabaseNameAsync([FromServices] DatabaseService databaseService)
    {
        return TypedResults.Ok(await databaseService.GetCorporateDatabaseNameAsync(DatabaseSchemaTypes.Rdb));
    }

    private static async Task<IResult> GetDatabaseNameAsync([FromServices] DatabaseService databaseService, [FromBody, Validate("Facility")] DatabaseRequest request)
    {
        return TypedResults.Ok(await databaseService.GetDatabaseNameAsync(request));
    }

    private static async Task<IResult> GetParentDatabaseNameAsync([FromServices] DatabaseService databaseService, [FromBody, Validate("ChildName")] DatabaseRequest request)
    {
        return TypedResults.Ok(await databaseService.GetParentDatabaseNameAsync(request));
    }
}
