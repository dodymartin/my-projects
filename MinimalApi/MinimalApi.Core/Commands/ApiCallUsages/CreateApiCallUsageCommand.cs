using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MinimalApi.Dom.ApiCallUsages.Dtos;

namespace MinimalApi.App.Commands.ApiCallUsages;

public record CreateApiCallUsageCommand(
    EndpointFilterInvocationContext Context, 
    EndpointFilterFactoryContext FilterFactoryContext, 
    long ElapsedMilliseconds)
    : IRequest<ErrorOr<ApiCallUsageDto>>;
