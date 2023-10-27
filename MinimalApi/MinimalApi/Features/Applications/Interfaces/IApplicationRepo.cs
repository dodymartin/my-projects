namespace MinimalApi.Api.Features.Applications;

public interface IApplicationRepo
{
    Task<Application?> GetApplicationAsync(int applicationId, CancellationToken cancellationToken);
    Task<Application?> GetApplicationAsync(string applicationName, CancellationToken cancellationToken);
    Task<string?> GetMinimumVersionAsync(int applicationId, int facilityId, CancellationToken cancellationToken);
}