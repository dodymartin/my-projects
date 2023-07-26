using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Core;

public class ApiCallUsageDto
{
    public Guid Id { get; set; }

    public string BasicUsername { get; set; }
    public byte[] Body { get; set; }
    public string CreateOrigin { get; set; }
    [NotMapped]
    public long ElapsedMilliseconds { get; set; }
    public bool HasAuthorizationHeader { get; set; }
    public string ApiApplicationExeName { get; set; }
    public string ApiApplicationVersion { get; set; }
    [NotMapped]
    public string ApiIpAddress { get; set; }
    public string ApiMachineName { get; set; }
    [NotMapped]
    public string ApiProcessId { get; set; }
    public string MethodName { get; set; }
    public string RequestApplicationExeName { get; set; }
    public string RequestApplicationVersion { get; set; }
    public string RequestIpAddress { get; set; }
    public string RequestMachineName { get; set; }
    public string RequestProcessId { get; set; }
    [NotMapped]
    public string Url { get; set; }

    #region Cast Operators

    public static explicit operator ApiCallUsage(ApiCallUsageDto from)
    {
        return new ApiCallUsage
        {
            ApiApplicationExeName = from.ApiApplicationExeName,
            ApiApplicationVersion = from.ApiApplicationVersion,
            ApiIpAddress = from.ApiIpAddress,
            ApiMachineName = from.ApiMachineName,
            ApiProcessId = from.ApiProcessId,
            BasicUsername = from.BasicUsername,
            Body = from.Body,
            CreateOrigin = from.CreateOrigin,
            ElapsedMilliseconds = from.ElapsedMilliseconds,
            HasAuthorizationHeader = from.HasAuthorizationHeader,
            Id = from.Id,
            MethodName = from.MethodName,
            RequestApplicationExeName = from.RequestApplicationExeName,
            RequestApplicationVersion = from.RequestApplicationVersion,
            RequestIpAddress = from.RequestIpAddress,
            RequestMachineName = from.RequestMachineName,
            RequestProcessId = from.RequestProcessId,
            Url = from.Url
        };
    }

    #endregion
}
