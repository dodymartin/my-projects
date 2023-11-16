using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Databases;

public sealed record GetCorporateQuery(
    DatabaseSchemaTypes DatabaseSchemaType)
    : IRequest<ErrorOr<string?>>;

public sealed class GetCorporateQueryValidator : AbstractValidator<GetCorporateQuery>
{
    public GetCorporateQueryValidator()
    {
        RuleFor(x => x.DatabaseSchemaType).NotEmpty();
    }
}

public sealed class GetCorporateQueryHandler(IOptions<AppSettings> appSettings, IDatabaseRepo databaseRepo) 
    : IRequestHandler<GetCorporateQuery, ErrorOr<string?>>
{
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IDatabaseRepo _databaseRepo = databaseRepo;

    public async Task<ErrorOr<string?>> Handle(GetCorporateQuery queryRequest, CancellationToken cancellationToken)
    {
        var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), _appSettings.EnvironmentType);
        var databaseName = await _databaseRepo.GetCorporateDatabaseNameAsync(environmentType, queryRequest.DatabaseSchemaType, cancellationToken);
        if (!string.IsNullOrEmpty(databaseName))
        {
            return databaseName!;
        }
        return Error.NotFound();
    }
}