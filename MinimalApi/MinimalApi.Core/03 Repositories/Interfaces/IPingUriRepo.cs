namespace MinimalApi.App.Interfaces;

public interface IPingUriRepo
{
    Task<IList<string>> GetPingUris(string applicationName, string applicationVersion);
}