using MinimalApi.Dal;

namespace MinimalApi.Repositories;

public interface IControllerUriFacilityInfoByApplicationRepo
{
    Task<IList<ControllerUriFacilityInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, int facilityId);
}