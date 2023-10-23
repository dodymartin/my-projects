using ErrorOr;
using MediatR;
using MinimalApi.Dom.Enumerations;

namespace MinimalApi.App.Queries.Databases;

public record GetCorporateQuery(
    DatabaseSchemaTypes DatabaseSchemaType)
    : IRequest<ErrorOr<string?>>;
