namespace MinimalApi.Core;

public interface IPingUriRepo
{
    Task<IList<string>> GetPingUris(string applicationName, string applicationVersion);
}