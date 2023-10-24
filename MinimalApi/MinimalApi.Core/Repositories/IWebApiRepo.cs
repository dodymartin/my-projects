using MinimalApi.Dom.Enumerations;
using MinimalApi.Dom.WebApis.Dtos;

namespace MinimalApi.App.Interfaces;

public interface IWebApiRepo
{
    Task<IList<ControllerUriInfoByApplicationDto>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, CancellationToken cancellationToken);
    Task<IList<ControllerUriInfoByApplicationDto>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, string machineName, CancellationToken cancellationToken);
    Task<IList<ControllerUriFacilityInfoByApplicationDto>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, int facilityId, CancellationToken cancellationToken);
    Task<WebApiVersionDto?> GetOneVersionAsync(int applicationId, string applicationVersion, CancellationToken cancellationToken);
    Task<IList<string>> GetPingUrisAsync(string applicationName, string applicationVersion, CancellationToken cancellationToken);
}
