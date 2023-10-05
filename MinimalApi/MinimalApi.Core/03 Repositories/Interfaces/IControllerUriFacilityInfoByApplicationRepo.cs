namespace MinimalApi.Core;

public interface IControllerUriFacilityInfoByApplicationRepo
{
    Task<IList<ControllerUriFacilityInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, ApplicationId applicationId, string applicationVersion, FacilityId facilityId);
}