using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text;

namespace MinimalApi.Api.Features.ApiCallUsages;

public class ApiCallUsageDto
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public string? BasicUsername { get; set; }
    public byte[]? Body { get; set; }
    public string? CreatedOrigin { get; set; }
    [NotMapped]
    public long ElapsedMilliseconds { get; set; }
    public bool HasAuthorizationHeader { get; set; }
    public string? ApiApplicationExeName { get; set; }
    public string? ApiApplicationVersion { get; set; }
    [NotMapped]
    public string? ApiIpAddress { get; set; }
    public string? ApiMachineName { get; set; }
    [NotMapped]
    public string? ApiProcessId { get; set; }
    public string? MethodName { get; set; }
    public string? RequestApplicationExeName { get; set; }
    public string? RequestApplicationVersion { get; set; }
    public string? RequestIpAddress { get; set; }
    public string? RequestMachineName { get; set; }
    public string? RequestProcessId { get; set; }
    [NotMapped]
    public string? Url { get; set; }

    #region Private Methods

    public static ApiCallUsageDto Create(EndpointFilterInvocationContext context, EndpointFilterFactoryContext filterFactoryContext, long elapsedMilliseconds)
    {
        var apiCallUsageDto = new ApiCallUsageDto
        {
            ApiIpAddress = context.HttpContext.Connection.LocalIpAddress?.ToString(),
            ApiProcessId = Environment.ProcessId.ToString(),
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
                var prettyBody = JsonSerializer.Serialize(jsonElement, _jsonOptions);

                apiCallUsageDto.Body = Encoding.ASCII.GetBytes(prettyBody);
            }
        }
        return apiCallUsageDto;
    }

    private static string? GetMachineName(string? ipAddress, string? defaultMachineName)
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
            var encodedUsernamePassword = authHeader!["Basic ".Length..].Trim();
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
