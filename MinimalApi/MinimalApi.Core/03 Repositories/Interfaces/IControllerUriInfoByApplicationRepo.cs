using MinimalApi.Dom.Enumerations;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

namespace MinimalApi.App.Interfaces;

public interface IControllerUriInfoByApplicationRepo
{
    Task<IList<ControllerUriInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, ApplicationId applicationId, string applicationVersion);
    Task<IList<ControllerUriInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, ApplicationId applicationId, string applicationVersion, string machineName);
}