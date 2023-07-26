using Stratos.Core;

namespace MinimalApi.Core
{
    public interface IDbContextSettings
    {
        IDatabase Database { get; }
        string? DatabaseName { get; set; }
    }
}