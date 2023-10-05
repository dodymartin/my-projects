namespace MinimalApi.Core;

public interface IControllerUriInfoByApplicationRepo
{
    Task<IList<ControllerUriInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, ApplicationId applicationId, string applicationVersion);
    Task<IList<ControllerUriInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, ApplicationId applicationId, string applicationVersion, string machineName);
}