using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Options;
using MinimalApi.App;
using MinimalApi.App.Commands.ApiCallUsages;

namespace MinimalApi.Api;

public class CallUsageFilter : IEndpointFilter
{
    private readonly ILogger<CallUsageFilter> _logger;
    private readonly IDbContextSettings _dbContextSettings;
    private readonly ISender _mediator;
    private readonly AppSettings _appSettings;
    private readonly EndpointFilterFactoryContext _filterFactoryContext;

    public CallUsageFilter(ILogger<CallUsageFilter> logger, IOptions<AppSettings> appSettings, IDbContextSettings dbContextSettings, ISender mediator, EndpointFilterFactoryContext filterFactoryContext)
    {
        _logger = logger;
        _dbContextSettings = dbContextSettings;
        _mediator = mediator;
        _appSettings = appSettings.Value;
        _filterFactoryContext = filterFactoryContext;
    }

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var sw = new Stopwatch();
        sw.Start();
        _dbContextSettings.DatabaseName = _appSettings.DatabaseName;
        var result = await next(context);
        sw.Stop();

        var apiCallUsageResponse = await _mediator.Send(new CreateApiCallUsageCommand(context, _filterFactoryContext, sw.ElapsedMilliseconds));

        if (apiCallUsageResponse.IsError)
        {
            apiCallUsageResponse.Errors.ForEach(x => _logger.LogWarning(x.Description));
            return result;
        }

        var apiCallUsageDto = apiCallUsageResponse.Value;
        _logger.LogDebug("{url}, {methodName}, {apiMachineName}, {apiIp}, {apiApp}, {apiAppVersion}, {apiProcessId}, {requestMachineName}, {requestIp}, {requestApp}, {requestAppVersion}, {requestProcessId}, {basicUser}, {hasAuthHeader}, {elapsedTime}",
            apiCallUsageDto.Url,
            apiCallUsageDto.MethodName,
            apiCallUsageDto.ApiMachineName,
            apiCallUsageDto.ApiIpAddress,
            apiCallUsageDto.ApiApplicationExeName,
            apiCallUsageDto.ApiApplicationVersion,
            apiCallUsageDto.ApiProcessId,
            apiCallUsageDto.RequestMachineName,
            apiCallUsageDto.RequestIpAddress,
            apiCallUsageDto.RequestApplicationExeName,
            apiCallUsageDto.RequestApplicationVersion,
            apiCallUsageDto.RequestProcessId,
            apiCallUsageDto.BasicUsername,
            apiCallUsageDto.HasAuthorizationHeader,
            apiCallUsageDto.ElapsedMilliseconds);
        return result;
    }
}
