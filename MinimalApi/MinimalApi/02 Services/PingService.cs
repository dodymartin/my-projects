using MinimalApi.Endpoints;

namespace MinimalApi.Services;

public class PingService
{
    private readonly UnitOfWorkService _uow;

    public PingService(UnitOfWorkService uow)
    {
        _uow = uow;
    }

    public async Task<IDictionary<string, string>> GetPingUrisAsync(PingUriRequest request)
    {
        var returnList = new Dictionary<string, string>();
        foreach (var serviceName in request.ServiceNames.Where(x => x.Contains("Web Api")))
        {
            var parts = serviceName.Split(" v");
            var applicationName = parts[0];
            var applicationVersion = Stratos.Core.CoreMethods.GetMajorMinorVersion(parts[1]);
            var pingUrls = await _uow.PingUriRepo.GetPingUris(applicationName, applicationVersion);
            foreach (var pingUrl in pingUrls)
                returnList.Add(pingUrl, serviceName);
        }
        return await Task.FromResult(returnList);
    }
}
