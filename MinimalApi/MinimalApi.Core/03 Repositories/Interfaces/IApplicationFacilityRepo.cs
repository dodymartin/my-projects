namespace MinimalApi.Core;

public interface IApplicationFacilityRepo
{
    Task<string> GetMinimumVersionAsync(int applicationId, int facilityId);
}