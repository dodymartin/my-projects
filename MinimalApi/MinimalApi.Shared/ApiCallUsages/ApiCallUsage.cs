using System.ComponentModel.DataAnnotations.Schema;
using MinimalApi.Dom.ApiCallUsages.ValueObjects;
using MinimalApi.Dom.Common.Models;

namespace MinimalApi.Dom.ApiCallUsages;

public class ApiCallUsage : AggregateRoot<ApiCallUsageId, Guid> //EntityBase<ApiCallUsage, ApiCallUsageId>
{
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

    public ApiCallUsage()
    {
        Id = ApiCallUsageId.CreateUnique();
    }
}
