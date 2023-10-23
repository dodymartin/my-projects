using ErrorOr;
using MediatR;

namespace MinimalApi.App.Queries.Databases;

public record GetNameQuery(
    int? FacilityId)
    : IRequest<ErrorOr<string?>>;
