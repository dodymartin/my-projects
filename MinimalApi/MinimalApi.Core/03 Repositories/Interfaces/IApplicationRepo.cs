namespace MinimalApi.Core
{
    public interface IApplicationRepo
    {
        Task<Application> GetApplicationAsync(int? applicationId, string applicationName);
        Task<string> GetMinimumVersionAsync(int applicationId);
    }
}