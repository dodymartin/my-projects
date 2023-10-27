using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public record GetControllerUrisQuery(
    int? ApplicationId,
    string? ApplicationName,
    string ApplicationVersion,
    string ControllerName,
    int? FacilityId,
    string? MachineName)
    : IRequest<ErrorOr<IList<GetControllerUrisResponse>>>;

public class GetControllerUrisQueryValidator : AbstractValidator<GetControllerUrisQuery>
{
    public GetControllerUrisQueryValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty()
            .Unless(x => !string.IsNullOrEmpty(x.ApplicationName));
        RuleFor(x => x.ApplicationName).NotEmpty()
            .Unless(x => x.ApplicationId.HasValue && x.ApplicationId.Value > 0);
        RuleFor(x => x.ApplicationVersion).NotEmpty();
    }
}

public class GetControllerUrisQueryHandler : IRequestHandler<GetControllerUrisQuery, ErrorOr<IList<GetControllerUrisResponse>>>
{
    private readonly AppSettings _appSettings;
    private readonly IWebApiRepo _webApiRepo;

    public GetControllerUrisQueryHandler(IOptions<AppSettings> appSettings,
        IWebApiRepo webApiRepo)
    {
        _appSettings = appSettings.Value;
        _webApiRepo = webApiRepo;
    }

    public async Task<ErrorOr<IList<GetControllerUrisResponse>>> Handle(GetControllerUrisQuery queryRequest, CancellationToken cancellationToken)
    {
        Application? application;
        if (queryRequest.ApplicationId.HasValue)
        {
            application = await _webApiRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, cancellationToken);
            if (application is null)
                return Error.NotFound(description: $"Application Id ({queryRequest.ApplicationId.Value}) is not found.");
        }
        else
        {
            application = await _webApiRepo.GetApplicationAsync(queryRequest.ApplicationName!, cancellationToken);
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