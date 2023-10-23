using ErrorOr;
using MediatR;

namespace MinimalApi.App.Queries.Databases;

public record GetParentQuery(
    string ChildDatabaseName)
    : IRequest<ErrorOr<string?>>;
