using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MinimalApi.Core;

public class ApiCallUsageService
{
    private readonly IBaseCrudRepo<ApiCallUsage, Guid> _apiCallUsageRepo;

    public ApiCallUsageService(IBaseCrudRepo<ApiCallUsage, Guid> apiCallUsageRepo)
    {
        _apiCallUsageRepo = apiCallUsageRepo;
    }

    public async Task<ApiCallUsageDom> CreateAsync(EndpointFilterInvocationContext context, EndpointFilterFactoryContext filterFactoryContext, long elapsedMilliseconds)
    {
        var apiCallUsageDto = GetDto(context, filterFactoryContext, elapsedMilliseconds);
        _apiCallUsageRepo.Insert((ApiCallUsage)apiCallUsageDto);
        await _apiCallUsageRepo.SaveAsync();
        return apiCallUsageDto;
    }

    #region Private Methods

    private static ApiCallUsageDom GetDto(EndpointFilterInvocationContext context, EndpointFilterFactoryContext filterFactoryContext, long elapsedMilliseconds)
    {
        var apiCallUsageDto = new ApiCallUsageDom
        {
            ApiIpAddress = context.HttpContext.Connection.LocalIpAddress?.ToString(),
            ApiProcessId = Environment.ProcessId.ToString(),
            Id = Guid.NewGuid(),
            MethodName = filterFactoryContext.MethodInfo.Name,
            RequestIpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString(),
            RequestProcessId = context.HttpContext.Request.Headers["RemoteProcessId"],
            RequestApplicationExeName = context.HttpContext.Request.Headers["RemoteExeName"],
            RequestApplicationVersion = context.HttpContext.Request.Headers["RemoteExeVersion"],
            ElapsedMilliseconds = elapsedMilliseconds,
            Url = context.HttpContext.Request.GetDisplayUrl()
        };
        apiCallUsageDto.ApiMachineName = GetMachineName(apiCallUsageDto.ApiIpAddress, apiCallUsageDto.ApiIpAddress);
        apiCallUsageDto.RequestMachineName = GetMachineName(apiCallUsageDto.RequestIpAddress, apiCallUsageDto.RequestIpAddress);

        (apiCallUsageDto.BasicUsername, apiCallUsageDto.HasAuthorizationHeader) = GetAuth(context.HttpContext.Request);

        var asm = Assembly.GetExecutingAssembly()?.GetName();
        if (asm != null)
        {
            apiCallUsageDto.ApiApplicationExeName = asm.Name + ".exe";
            apiCallUsageDto.ApiApplicationVersion = asm.Version?.ToString();
        }

        if (context.HttpContext.Request.Body != null)
        {
            using var bodyStream = new StreamReader(context.HttpContext.Request.Body);

            // Format the json with indentation
            var uglyBody = bodyStream.ReadToEndAsync().Result;
            if (!string.IsNullOrEmpty(uglyBody))
            {
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(uglyBody);
                var prettyBody = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions() { WriteIndented = true });

                apiCallUsageDto.Body = Encoding.ASCII.GetBytes(prettyBody);
            }
        }
        return apiCallUsageDto;
    }

    private static string GetMachineName(string ipAddress, string defaultMachineName)
    {
        var machineName = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(ipAddress))
            {
                machineName = Dns.GetHostEntry(ipAddress).HostName;
                if (!string.IsNullOrEmpty(machineName))
                    machineName = machineName.Split('.')[0];
                else
                    machineName = defaultMachineName;
            }
        }
        catch (SocketException)
        {
            machineName = defaultMachineName;
        }
        return machineName;
    }

    private static (string basicUser, bool hasToken) GetAuth(HttpRequest request)
    {
        // Parse username value from "Authorization" header
        var hasAuthorizationHeader = false;
        var username = string.Empty;
        if (request != null && request.Headers.TryGetValue("Authorization", out var authToken))
        {
            hasAuthorizationHeader = true;
            var authHeader = authToken.First();
            var encodedUsernamePassword = authHeader["Basic ".Length..].Trim();
            if (authHeader.StartsWith("Basic"))
            {
                var tokenParts =
                    Encoding.UTF8.GetString(
                        Convert.FromBase64String(
                            encodedUsernamePassword))
                    .Split(':');
                if (tokenParts.Length >= 1)
                    username = tokenParts[0];
            }
        }
        return (username, hasAuthorizationHeader);
    }

    #endregion
}
