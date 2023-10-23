using ErrorOr;
using MediatR;
using MinimalApi.App.Interfaces;

namespace MinimalApi.App.Queries.WebApis;

public class GetPingUrisQueryHandler : IRequestHandler<GetPingUrisQuery, ErrorOr<IDictionary<string, string>>>
{
    private readonly IWebApiRepo _webApiRepo;

    public GetPingUrisQueryHandler(IWebApiRepo webApiRepo)
    {
        _webApiRepo = webApiRepo;
    }

    public async Task<ErrorOr<IDictionary<string, string>>> Handle(GetPingUrisQuery queryRequest, CancellationToken cancellationToken)
    {
        var returnList = new Dictionary<string, string>();
        foreach (var serviceName in queryRequest.ServiceNames.Where(x => x.Contains("Web Api")))
        {
            var parts = serviceName.Split(" v");
            var applicationName = parts[0];
            var applicationVersion = Stratos.Core.CoreMethods.GetMajorMinorVersion(parts[1]);

            var pingUrls = await _webApiRepo.GetPingUrisAsync(applicationName, applicationVersion, cancellationToken);
            foreach (var pingUrl in pingUrls)
                returnList.Add(pingUrl, serviceName);
        }
        return returnList;
    }
}
