using ErrorOr;
using MediatR;
using MinimalApi.App.Interfaces;

namespace MinimalApi.App.Queries.Databases;

public class GetParentQueryHandler : IRequestHandler<GetParentQuery, ErrorOr<string?>>
{
    private readonly IDatabaseRepo _databaseRepo;

    public GetParentQueryHandler(IDatabaseRepo databaseRepo)
    {
        _databaseRepo = databaseRepo;
    }

    public async Task<ErrorOr<string?>> Handle(GetParentQuery request, CancellationToken cancellationToken)
    {
        var databaseName = await _databaseRepo.GetParentDatabaseNameAsync(request.ChildDatabaseName, cancellationToken);
        if (!string.IsNullOrEmpty(databaseName))
        {
            return databaseName!;
        }
        return Error.NotFound();
    }
}
