using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MinimalApi.Version;

namespace MinimalApi;

public static class VersionWebApi
{
    public static void UseVersionWebApi(this WebApplication app)
    {
        var group = app.MapGroup("api/version/v6");

        group.MapGet("/ping", PingAsync)
            .WithName(nameof(PingAsync));

        group.MapGet("/applications/{applicationId:int}/versions/{version}/check-minimum-version",
            CheckMinimumVersionByIdAsync)
            .WithName(nameof(CheckMinimumVersionByIdAsync));

        group.MapGet("/applications/{applicationId:int}/versions/{version}/facilities/{facilityId:int}/check-minimum-version",
            CheckMinimumVersionByIdAndFacilityAsync)
            .WithName(nameof(CheckMinimumVersionByIdAndFacilityAsync));

        group.MapGet("/application-names/{applicationName}/versions/{version}/check-minimum-version",
            CheckMinimumVersionByNameAsync)
            .WithName(nameof(CheckMinimumVersionByNameAsync));

        group.MapGet("/application-names/{applicationName}/versions/{version}/facilities/{facilityId:int}/check-minimum-version",
            CheckMinimumVersionByNameAndFacilityAsync)
            .WithName(nameof(CheckMinimumVersionByNameAndFacilityAsync));
    }

    private static async Task<IResult> PingAsync([FromServices] IOptions<AppSettings> appSettings, [FromServices] IVersionService versionService)
    {
        versionService.ContextSettings.DatabaseName = appSettings.Value.DatabaseName;
        versionService.WriteCollections();
        return TypedResults.Ok(
            await Task.FromResult("Ping Successful!"));
    }

    private static async Task<IResult> CheckMinimumVersionByIdAsync([FromServices] IOptions<AppSettings> appSettings, [FromServices] VersionDataContextSettings dataContextSettings, [FromServices] IVersionService versionService, int applicationId, string version)
    {
        versionService.ContextSettings.DatabaseName = appSettings.Value.DatabaseName;
        return TypedResults.Ok(
            await Task.FromResult(
                versionService.MinimumVersion(applicationId, version)));
    }

    private static async Task<IResult> CheckMinimumVersionByIdAndFacilityAsync([FromServices] IOptions<AppSettings> appSettings, [FromServices] IVersionService versionService, int applicationId, string version, int facilityId)
    {
        versionService.ContextSettings.DatabaseName = appSettings.Value.DatabaseName;
        return TypedResults.Ok(
            await Task.FromResult(
                versionService.MinimumVersion(applicationId, version, facilityId)));
    }

    private static async Task<IResult> CheckMinimumVersionByNameAsync([FromServices] IOptions<AppSettings> appSettings, [FromServices] IVersionService versionService, string applicationName, string version)
    {
        versionService.ContextSettings.DatabaseName = appSettings.Value.DatabaseName;
        return TypedResults.Ok(
            await Task.FromResult(
                versionService.MinimumVersion(applicationName, version)));
    }

    private static async Task<IResult> CheckMinimumVersionByNameAndFacilityAsync([FromServices] IOptions<AppSettings> appSettings, [FromServices] IVersionService versionService, string applicationName, string version, int facilityId)
    {
        versionService.ContextSettings.DatabaseName = appSettings.Value.DatabaseName;
        return TypedResults.Ok(
            await Task.FromResult(
                versionService.MinimumVersion(applicationName, version, facilityId)));
    }
}
