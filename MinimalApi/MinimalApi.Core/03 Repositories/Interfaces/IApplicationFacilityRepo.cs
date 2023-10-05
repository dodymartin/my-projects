namespace MinimalApi.Core;

public interface IApplicationFacilityRepo
{
    Task<string> GetMinimumVersionAsync(ApplicationId applicationId, FacilityId facilityId);
}