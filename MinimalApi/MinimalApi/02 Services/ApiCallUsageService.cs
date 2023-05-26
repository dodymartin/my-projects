﻿using Microsoft.AspNetCore.Http.Extensions;
using MinimalApi.Repositories;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text;
using MinimalApi.Dal;

namespace MinimalApi.Services;

public class ApiCallUsageService
{
    private readonly UnitOfWorkService _uow;
    private readonly DbContextSettings _dbContextSettings;

    public ApiCallUsageService(UnitOfWorkService uow, DbContextSettings dbContextSettings)
    {
        _uow = uow;
        _dbContextSettings = dbContextSettings;
    }

    public ApiCallUsageDto Create(string databaseName, EndpointFilterInvocationContext context, EndpointFilterFactoryContext filterFactoryContext, long elapsedMilliseconds)
    {
        _dbContextSettings.DatabaseName = databaseName;

        var apiCallUsageDto = GetDto(context, filterFactoryContext, elapsedMilliseconds);
        _uow.ApiCallUsageRepo.Insert((ApiCallUsage)apiCallUsageDto);
        _uow.Save();
        return apiCallUsageDto;
    }

    #region Private Methods

    private static ApiCallUsageDto GetDto(EndpointFilterInvocationContext context, EndpointFilterFactoryContext filterFactoryContext, long elapsedMilliseconds)
    {
        var apiCallUsageDto = new ApiCallUsageDto
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
