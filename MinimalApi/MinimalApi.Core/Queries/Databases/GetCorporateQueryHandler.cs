using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using MinimalApi.App.Interfaces;
using MinimalApi.Dom.Enumerations;

namespace MinimalApi.App.Queries.Databases;

public class GetCorporateQueryHandler : IRequestHandler<GetCorporateQuery, ErrorOr<string?>>
{
    private readonly AppSettings _appSettings;
    private readonly IDatabaseRepo _databaseRepo;

    public GetCorporateQueryHandler(IOptions<AppSettings> appSettings, IDatabaseRepo databaseRepo)
    {
        _appSettings = appSettings.Value;
        _databaseRepo = databaseRepo;
    }

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
