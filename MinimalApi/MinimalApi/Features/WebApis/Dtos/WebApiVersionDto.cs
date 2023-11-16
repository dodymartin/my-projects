namespace MinimalApi.Api.Features.WebApis;

public sealed class WebApiVersionDto
{
    public required int ApplicationId { get; init; }
    public required int Port { get; set; }
    public required bool UseHttps { get; init; }
    public required string Version { get; init; }
    public required int WebApiId { get; init; }

    public string GetBaseUri() => $@"http{(UseHttps ? "s" : "")}://+:{Port}";
}