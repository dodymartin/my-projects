using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public interface IWebApiRepo
{
    Task<Application?> GetApplicationAsync(int applicationId, CancellationToken cancellationToken);
    Task<Application?> GetApplicationAsync(string applicationName, CancellationToken cancellationToken);
    Task<IList<ControllerUriInfoByApplicationDto>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, CancellationToken cancellationToken);
    Task<IList<ControllerUriInfoByApplicationDto>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, string machineName, CancellationToken cancellationToken);
    Task<IList<ControllerUriFacilityInfoByApplicationDto>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, int facilityId, CancellationToken cancellationToken);
    Task<WebApiVersionDto?> GetOneVersionAsync(int applicationId, string applicationVersion, CancellationToken cancellationToken);
    Task<IList<string>> GetPingUrisAsync(string applicationName, string applicationVersion, CancellationToken cancellationToken);
}
