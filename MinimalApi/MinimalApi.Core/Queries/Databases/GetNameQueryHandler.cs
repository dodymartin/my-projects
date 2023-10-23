using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using MinimalApi.App.Interfaces;
using MinimalApi.Dom.Enumerations;

namespace MinimalApi.App.Queries.Databases;

public class GetNameQueryHandler : IRequestHandler<GetNameQuery, ErrorOr<string?>>
{
    private readonly AppSettings _appSettings;
    private readonly IDatabaseRepo _databaseRepo;

    public GetNameQueryHandler(IOptions<AppSettings> appSettings, IDatabaseRepo databaseRepo)
    {
        _appSettings = appSettings.Value;
        _databaseRepo = databaseRepo;
    }

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
