using Stratos.Core;

namespace MinimalApi.App;

public interface IDbContextSettings
{
    IDatabase Database { get; }
    string? DatabaseName { get; set; }
}