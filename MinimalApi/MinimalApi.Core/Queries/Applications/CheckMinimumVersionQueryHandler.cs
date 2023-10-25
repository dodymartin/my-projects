using ErrorOr;
using MediatR;
using MinimalApi.App.Interfaces;
using MinimalApi.Dom.Applications;

namespace MinimalApi.App.Queries.Applications;

public class CheckMinimumVersionQueryHandler : IRequestHandler<CheckMinimumVersionQuery, ErrorOr<bool>>
{
    private readonly IApplicationRepo _applicationRepo;

    public CheckMinimumVersionQueryHandler(IApplicationRepo applicationRepo)
    {
        _applicationRepo = applicationRepo;
    }

    public async Task<ErrorOr<bool>> Handle(CheckMinimumVersionQuery queryRequest, CancellationToken cancellationToken)
    {
        Application? application;
        if (queryRequest.ApplicationId.HasValue)
        {
            application = await _applicationRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, cancellationToken);
            if (application is null)
                return Error.NotFound(description: $"Application ({queryRequest.ApplicationId.Value}) is not found.");
        }
        else
        {
            application = await _applicationRepo.GetApplicationAsync(queryRequest.ApplicationName!, cancellationToken);
            if (application is null)
                return Error.NotFound(description: $"Application ({queryRequest.ApplicationName}) is not found.");
        }

        var minimumVersion = application.MinimumAssemblyVersion;
        if (queryRequest.FacilityId.HasValue)
            minimumVersion = await _applicationRepo.GetMinimumVersionAsync(application.Id.Value, queryRequest.FacilityId.Value, cancellationToken);

        return Application.CheckVersion(minimumVersion, queryRequest.ApplicationVersion);
    }
}
