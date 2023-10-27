﻿using ErrorOr;
using FluentValidation;
using MediatR;

namespace MinimalApi.Api.Features.WebApis;

public record GetBaseUriQuery(
    int? ApplicationId,
    string? ApplicationVersion)
    : IRequest<ErrorOr<string?>>;

public class GetBaseUriQueryValidator : AbstractValidator<GetBaseUriQuery>
{
    public GetBaseUriQueryValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
        RuleFor(x => x.ApplicationVersion).NotEmpty();
    }
}

public class GetBaseUriQueryHandler : IRequestHandler<GetBaseUriQuery, ErrorOr<string?>>
{
    private readonly IWebApiRepo _webApiRepo;

    public GetBaseUriQueryHandler(IWebApiRepo webApiRepo)
    {
        _webApiRepo = webApiRepo;
    }

    public async Task<ErrorOr<string?>> Handle(GetBaseUriQuery queryRequest, CancellationToken cancellationToken)
    {
        var webApiVersionDto = await _webApiRepo.GetOneVersionAsync(queryRequest.ApplicationId!.Value, queryRequest.ApplicationVersion!, cancellationToken);
        if (webApiVersionDto is not null)
        {
            return webApiVersionDto.GetBaseUri();
        }
        return Error.NotFound(description: $"A Web Api version for application ({queryRequest.ApplicationId!.Value}, {queryRequest.ApplicationVersion}) is not found.");
    }
}