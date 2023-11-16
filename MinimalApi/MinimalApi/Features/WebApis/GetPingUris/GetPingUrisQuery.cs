﻿using ErrorOr;
using FluentValidation;
using MediatR;
using MinimalApi.Api.Core;

namespace MinimalApi.Api.Features.WebApis;

public sealed record GetPingsQuery(
    string[] ServiceNames)
    : IRequest<ErrorOr<IDictionary<string, string>>>;

public sealed class GetPingUrisQueryValidator : AbstractValidator<GetPingsQuery>
{
    public GetPingUrisQueryValidator()
    {
        RuleFor(x => x.ServiceNames).NotEmpty();
    }
}

public sealed class GetPingUrisQueryHandler(IWebApiRepo webApiRepo) 
    : IRequestHandler<GetPingsQuery, ErrorOr<IDictionary<string, string>>>
{
    private readonly IWebApiRepo _webApiRepo = webApiRepo;

    public async Task<ErrorOr<IDictionary<string, string>>> Handle(GetPingsQuery queryRequest, CancellationToken cancellationToken)
    {
        var returnList = new Dictionary<string, string>();
        foreach (var serviceName in queryRequest.ServiceNames.Where(x => x.Contains("Web Api")))
        {
            var parts = serviceName.Split(" v");
            var applicationName = parts[0];
            var applicationVersion = CoreMethods.GetMajorMinorVersion(parts[1]);

            var pingUrls = await _webApiRepo.GetPingUrisAsync(applicationName, applicationVersion, cancellationToken);
            foreach (var pingUrl in pingUrls)
                returnList.Add(pingUrl, serviceName);
        }
        return returnList;
    }
}