using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public class Application : AggregateRoot<ApplicationId, int> //EntityBase<Application, ApplicationId>
{
    public string Name { get; set; }
}
