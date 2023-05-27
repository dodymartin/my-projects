namespace MinimalApi.Repositories
{
    public interface IApplicationRepo
    {
        Task<ApplicationDto> GetApplicationAsync(int? applicationId, string applicationName);
        Task<string> GetMinimumVersionAsync(int applicationId);
    }
}