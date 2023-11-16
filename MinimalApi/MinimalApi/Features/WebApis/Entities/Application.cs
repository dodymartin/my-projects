using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public sealed class Application : AggregateRoot<ApplicationId, int> //EntityBase<Application, ApplicationId>
{
    public required string Name { get; set; }
}
