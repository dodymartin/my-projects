using System.Collections.ObjectModel;

namespace MinimalApi.Api.Core;

public class DatabasesLoader : KeyedCollection<string, DatabaseInfo>, IDatabases
{
    public DatabasesLoader()
        : base(StringComparer.CurrentCultureIgnoreCase)
    { }

    public new IDatabase this[string databaseName]
    {
        get
        {
#if NET6_0_OR_GREATER
            if (TryGetValue(databaseName, out var database))
                return database;
#else
            if (Contains(databaseName))
                return base[databaseName];
#endif
            throw new Exception($"The database name, {databaseName}, is not found!");
        }
    }

    protected override string GetKeyForItem(DatabaseInfo item)
    {
        return item.Name;
    }

    public static IDatabases Load(string configPath)
        => CoreMethods.FindAndLoad<DatabasesLoader>(configPath, "databases.config.json")!;

    public static IDatabases Load(string configPath, string sitePath)
        => CoreMethods.FindAndLoad<DatabasesLoader>(configPath, sitePath, "databases.config.json")!;
}