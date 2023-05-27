﻿using Stratos.Core;

namespace MinimalApi.Dal;

public class DbContextSettings
{
    private readonly IDatabases _dbs;

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

    public DbContextSettings(IDatabases dbs)
    {
        _dbs = dbs;
    }
}
