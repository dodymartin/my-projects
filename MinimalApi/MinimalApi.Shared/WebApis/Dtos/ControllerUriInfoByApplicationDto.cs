using MinimalApi.Dom.Enumerations;

namespace MinimalApi.Dom.WebApis.Dtos;

public record ControllerUriInfoByApplicationDto(
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