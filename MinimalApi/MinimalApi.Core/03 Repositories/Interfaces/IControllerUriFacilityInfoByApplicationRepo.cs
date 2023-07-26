namespace MinimalApi.Core;

public interface IControllerUriFacilityInfoByApplicationRepo
{
    Task<IList<ControllerUriFacilityInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, int facilityId);
}