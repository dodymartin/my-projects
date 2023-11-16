using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public sealed record ControllerUriInfoByApplicationDto(
    string Address,
    int ApplicationId,
    string ApplicationName,
    string ApplicationVersion,
    EnvironmentTypes EnvironmentType,
    string? MachineName,
    int Order,
    int Port,
    string UriName,
    bool UseHttps,
    string Version);