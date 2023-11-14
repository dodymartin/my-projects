using MinimalApi.Api.Core;

namespace MinimalApi.Api.Common;

public interface IDbContextSettings
{
    IDatabase Database { get; }
    string? DatabaseName { get; set; }
}