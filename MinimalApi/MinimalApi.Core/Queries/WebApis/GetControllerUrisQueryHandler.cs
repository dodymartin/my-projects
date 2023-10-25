using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using MinimalApi.App.Interfaces;
using MinimalApi.Dom.Applications;
using MinimalApi.Dom.Enumerations;

namespace MinimalApi.App.Queries.WebApis;

public class GetControllerUrisQueryHandler : IRequestHandler<GetControllerUrisQuery, ErrorOr<IList<GetControllerUrisResponse>>>
{
    private readonly AppSettings _appSettings;
    private readonly IApplicationRepo _applicationRepo;
    private readonly IWebApiRepo _webApiRepo;

    public GetControllerUrisQueryHandler(IOptions<AppSettings> appSettings,
        IApplicationRepo applicationRepo,
        IWebApiRepo webApiRepo)
    {
        _appSettings = appSettings.Value;
        _applicationRepo = applicationRepo;
        _webApiRepo = webApiRepo;
    }

    public async Task<ErrorOr<IList<GetControllerUrisResponse>>> Handle(GetControllerUrisQuery queryRequest, CancellationToken cancellationToken)
    {
        Application? application;
        if (queryRequest.ApplicationId.HasValue)
        {
            application = await _applicationRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, cancellationToken);
            if (application is null)
                return Error.NotFound(description: $"Application Id ({queryRequest.ApplicationId.Value}) is not found.");
        }
        else
        {
            application = await _applicationRepo.GetApplicationAsync(queryRequest.ApplicationName!, cancellationToken);
            if (application is null)
                return Error.NotFound(description: $"Application Name ({queryRequest.ApplicationName}) is not found.");
        }

        var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), _appSettings.EnvironmentType);

        if (queryRequest.FacilityId.HasValue)
        {
            return
                (await _webApiRepo.GetControllerUrisAsync(environmentType, queryRequest.ControllerName, application.Id.Value, queryRequest.ApplicationVersion, queryRequest.FacilityId.Value, cancellationToken))
                .OrderBy(x => x.Order)
                .Select(Mapper.MapToControllerUrisResponse)
                .ToList();
        }
        else if (!string.IsNullOrEmpty(queryRequest.MachineName))
        {
            return
                (await _webApiRepo.GetControllerUrisAsync(environmentType, queryRequest.ControllerName, application.Id.Value, queryRequest.ApplicationVersion, queryRequest.MachineName, cancellationToken))
                .OrderBy(x => x.Order)
                .Select(Mapper.MapToControllerUrisResponse)
                .ToList();
        }
        else
        {
            return
                (await _webApiRepo.GetControllerUrisAsync(environmentType, queryRequest.ControllerName, application.Id.Value, queryRequest.ApplicationVersion, cancellationToken))
                .OrderBy(x => x.Order)
                .Select(Mapper.MapToControllerUrisResponse)
                .ToList();
        }
    }
}

