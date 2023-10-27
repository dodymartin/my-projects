using ErrorOr;
using FluentValidation;
using MediatR;

namespace MinimalApi.Api.Features.Applications;

public record CheckMinimumVersionQuery(
    int? ApplicationId,
    string? ApplicationName,
    string ApplicationVersion,
    int? FacilityId)
    : IRequest<ErrorOr<bool>>;

public class CheckMinimumVersionQueryValidator : AbstractValidator<CheckMinimumVersionQuery>
{
    public CheckMinimumVersionQueryValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty()
            .Unless(x => !string.IsNullOrEmpty(x.ApplicationName));
        RuleFor(x => x.ApplicationName).NotEmpty()
            .Unless(x => x.ApplicationId.HasValue && x.ApplicationId.Value > 0);
        RuleFor(x => x.ApplicationVersion).NotEmpty();
    }
}

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
                return Error.NotFound(description: $"Application for id ({queryRequest.ApplicationId.Value}) is not found.");
        }
        else
        {
            application = await _applicationRepo.GetApplicationAsync(queryRequest.ApplicationName!, cancellationToken);
            if (application is null)
                return Error.NotFound(description: $"Application for name ({queryRequest.ApplicationName}) is not found.");
        }

        var minimumVersion = application.MinimumAssemblyVersion;
        if (queryRequest.FacilityId.HasValue)
            minimumVersion = await _applicationRepo.GetMinimumVersionAsync(application.Id.Value, queryRequest.FacilityId.Value, cancellationToken);

        return Application.CheckVersion(minimumVersion, queryRequest.ApplicationVersion);
    }
}