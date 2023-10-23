using ErrorOr;
using MediatR;

namespace MinimalApi.App.Queries.WebApis;

public record GetBaseUriQuery(
    int? ApplicationId,
    string? ApplicationVersion)
    : IRequest<ErrorOr<string?>>;
