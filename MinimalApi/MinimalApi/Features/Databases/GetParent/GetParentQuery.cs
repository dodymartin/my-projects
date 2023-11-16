using ErrorOr;
using FluentValidation;
using MediatR;

namespace MinimalApi.Api.Features.Databases;

public sealed record GetParentQuery(
    string ChildDatabaseName)
    : IRequest<ErrorOr<string?>>;

public sealed class GetParentQueryValidator : AbstractValidator<GetParentQuery>
{
    public GetParentQueryValidator()
    {
        RuleFor(x => x.ChildDatabaseName).NotEmpty();
    }
}

public sealed class GetParentQueryHandler(IDatabaseRepo databaseRepo) 
    : IRequestHandler<GetParentQuery, ErrorOr<string?>>
{
    private readonly IDatabaseRepo _databaseRepo = databaseRepo;

    public async Task<ErrorOr<string?>> Handle(GetParentQuery request, CancellationToken cancellationToken)
    {
        var databaseName = await _databaseRepo.GetParentDatabaseNameAsync(request.ChildDatabaseName, cancellationToken);
        if (!string.IsNullOrEmpty(databaseName))
        {
            return databaseName!;
        }
        return Error.NotFound(description: $"Parent for {request.ChildDatabaseName} is not found.");
    }
}