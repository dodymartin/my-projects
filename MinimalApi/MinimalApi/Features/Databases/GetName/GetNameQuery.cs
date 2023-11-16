using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Databases;

public sealed record GetNameQuery(
    int? FacilityId)
    : IRequest<ErrorOr<string?>>;

public sealed class GetNameQueryValidator : AbstractValidator<GetNameQuery>
{
    public GetNameQueryValidator()
    {
        RuleFor(x => x.FacilityId).NotEmpty();
    }
}

public sealed class GetNameQueryHandler(IOptions<AppSettings> appSettings, IDatabaseRepo databaseRepo) 
    : IRequestHandler<GetNameQuery, ErrorOr<string?>>
{
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IDatabaseRepo _databaseRepo = databaseRepo;

    public async Task<ErrorOr<string?>> Handle(GetNameQuery queryRequest, CancellationToken cancellationToken)
    {
        var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), _appSettings.EnvironmentType);
        var databaseName = await _databaseRepo.GetDatabaseNameAsync(environmentType, queryRequest.FacilityId!.Value, cancellationToken);
        if (!string.IsNullOrEmpty(databaseName))
        {
            return databaseName!;
        }
        return Error.NotFound();
    }
}