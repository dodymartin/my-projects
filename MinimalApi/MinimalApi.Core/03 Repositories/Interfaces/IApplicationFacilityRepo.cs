using MinimalApi.Dom.Facilities.ValueObjects;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

namespace MinimalApi.App.Interfaces;

public interface IApplicationFacilityRepo
{
    Task<string?> GetMinimumVersionAsync(ApplicationId applicationId, FacilityId facilityId);
}