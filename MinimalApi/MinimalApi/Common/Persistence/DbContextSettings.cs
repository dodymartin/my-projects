using MinimalApi.Api.Core;

namespace MinimalApi.Api.Common;

public class DbContextSettings(IDatabases dbs) : IDbContextSettings
{
    private readonly IDatabases _dbs = dbs;

    public string? DatabaseName { get; set; }
    public IDatabase Database
    {
        get
        {
            if (string.IsNullOrEmpty(DatabaseName))
                throw new ArgumentNullException(nameof(DatabaseName));

            try
            {
                return _dbs[DatabaseName];
            }
            catch (Exception ex)
            {
                throw new Exception($"{DatabaseName} is not found. {ex}");
            }
        }
    }
}
