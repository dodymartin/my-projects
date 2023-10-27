using Carter;

namespace MinimalApi.Api.Features.WebApis;

public class GetPingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder parentGroup)
    {
        var group = parentGroup.MapGroup("/ping");

        group.MapGet("/",
            PingAsync);
    }

    public static async Task<IResult> PingAsync()
    {
        return TypedResults.Ok(await Task.FromResult("Ping Successful!"));
    }
}
