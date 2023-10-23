using MinimalApi.Dom.WebApis.Entities;

namespace MinimalApi.Dom.WebApis.Dtos;

public class WebApiDto
{
    public int Id { get; init; }

    public int ApplicationId { get; init; }
    public bool UseHttps { get; init; }

    public IReadOnlyList<WebApiController>? Controllers { get; init; }
    public IReadOnlyList<WebApiVersion>? Versions { get; init; }

    public string? GetBaseUriByApplicationVersion(bool useHttps, string applicationVersion)
    {
        var version = Versions?.FirstOrDefault(x =>
            applicationVersion.StartsWith(x.Version[..applicationVersion.Length]));
        if (version is not null)
            return $@"http{(useHttps ? "s" : "")}://+:{version.Port}";
        return default;
    }
}