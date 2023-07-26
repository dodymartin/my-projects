namespace MinimalApi.Core;

public interface IControllerUriInfoByApplicationRepo
{
    Task<IList<ControllerUriInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion);
    Task<IList<ControllerUriInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, string machineName);
}