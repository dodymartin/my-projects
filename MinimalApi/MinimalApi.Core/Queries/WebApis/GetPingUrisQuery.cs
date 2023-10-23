using ErrorOr;
using MediatR;

namespace MinimalApi.App.Queries.WebApis;

public record GetPingUrisQuery(
    string[] ServiceNames)
    : IRequest<ErrorOr<IDictionary<string, string>>>;
