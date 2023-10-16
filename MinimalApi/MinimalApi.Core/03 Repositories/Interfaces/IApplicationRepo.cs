using MinimalApi.Dom.Applications;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

namespace MinimalApi.App.Interfaces
{
    public interface IApplicationRepo
    {
        Task<Application?> GetApplicationAsync(ApplicationId? applicationId, string applicationName);
        Task<string?> GetMinimumVersionAsync(ApplicationId applicationId);
    }
}