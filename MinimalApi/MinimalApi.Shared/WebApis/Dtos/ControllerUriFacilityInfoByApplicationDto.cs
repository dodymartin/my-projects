using MinimalApi.Dom.Enumerations;

namespace MinimalApi.Dom.WebApis.Dtos;

public record ControllerUriFacilityInfoByApplicationDto(
    string Address,
    int ApplicationId,
    string ApplicationName,
    string ApplicationVersion,
    EnvironmentTypes EnvironmentType,
    int FacilityId,
    int Order,
    int Port,
    string UriName,
    bool UseHttps,
    string Version);