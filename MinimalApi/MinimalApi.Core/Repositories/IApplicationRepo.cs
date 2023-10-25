using MinimalApi.Dom.Applications;

namespace MinimalApi.App.Interfaces
{
    public interface IApplicationRepo
    {
        Task<Application?> GetApplicationAsync(int applicationId, CancellationToken cancellationToken);
        Task<Application?> GetApplicationAsync(string applicationName, CancellationToken cancellationToken);
        Task<string?> GetMinimumVersionAsync(int applicationId, int facilityId, CancellationToken cancellationToken);
    }
}