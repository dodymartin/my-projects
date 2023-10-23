using MinimalApi.Dom.Common.Models;

namespace MinimalApi.Dom.ApiCallUsages.Events;

public record ApiCallUsageCreated(ApiCallUsage apiCallUsage) : IDomainEvent;