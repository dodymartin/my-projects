namespace MinimalApi.Core
{
    public interface IApplicationRepo
    {
        Task<Application> GetApplicationAsync(ApplicationId? applicationId, string applicationName);
        Task<string> GetMinimumVersionAsync(ApplicationId applicationId);
    }
}