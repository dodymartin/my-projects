using ErrorOr;
using MediatR;

namespace MinimalApi.App.Queries.WebApis;

public record GetControllerUrisQuery(
    int? ApplicationId,
    string? ApplicationName,
    string ApplicationVersion,
    string ControllerName,
    int? FacilityId,
    string? MachineName)
    : IRequest<ErrorOr<IList<GetControllerUrisResponse>>>;
