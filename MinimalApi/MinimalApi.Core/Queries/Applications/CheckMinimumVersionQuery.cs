using ErrorOr;
using MediatR;

namespace MinimalApi.App.Queries.Applications;

public record CheckMinimumVersionQuery(
    int? ApplicationId,
    string? ApplicationName,
    string ApplicationVersion,
    int? FacilityId)
    : IRequest<ErrorOr<bool>>;
