namespace MinimalApi.Repositories
{
    public interface IApplicationFacilityRepo
    {
        Task<string> GetMinimumVersionAsync(int applicationId, int facilityId);
    }
}