using System.Diagnostics;
using Microsoft.Extensions.Options;
using MinimalApi.Services;

namespace MinimalApi;

public class CallUsageFilter : IEndpointFilter
{
    private readonly ILogger<CallUsageFilter> _logger;
    private readonly AppSettings _appSettings;
    private readonly ApiCallUsageService _apiCallUsageService;
    private readonly EndpointFilterFactoryContext _filterFactoryContext;

    public CallUsageFilter(ILogger<CallUsageFilter> logger, IOptions<AppSettings> appSettings, ApiCallUsageService apiCallUsageService, EndpointFilterFactoryContext filterFactoryContext)
    {
        _logger = logger;
        _appSettings = appSettings.Value;
        _apiCallUsageService = apiCallUsageService;
        _filterFactoryContext = filterFactoryContext;
    }

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var sw = new Stopwatch();
        sw.Start();
        var result = await next(context);
        sw.Stop();

        var apiCallUsageDto = _apiCallUsageService.Create(_appSettings.DatabaseName, context, _filterFactoryContext, sw.ElapsedMilliseconds);
        
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
