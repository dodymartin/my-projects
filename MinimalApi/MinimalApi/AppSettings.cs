using System.ComponentModel.DataAnnotations;
using MinimalApi.Api.Common;

namespace MinimalApi.Api;

public class AppSettings
{
    public const string ConfigurationSection = "MinimalApi.Api.AppSettings";

    [Required]
    public string DatabaseName { get; set; }
    [Required, EnumDataType(typeof(EnvironmentTypes))]
    public string EnvironmentType { get; set; }
    public bool LogCallers { get; set; } = true;
    public string NamesDirectoryPath
    {
        get => Oracle.ManagedDataAccess.Client.OracleConfiguration.NamesDirectoryPath ?? string.Empty;
        set
        {
            bool.TryParse(Environment.GetEnvironmentVariable("DOCKER_RUNNING"), out var dockerRunning);
            if (dockerRunning)
            {
                var namesDirectoryPath = Environment.GetEnvironmentVariable("ORACLE_NAMES_DIR_PATH");
                if (!string.IsNullOrEmpty(namesDirectoryPath))
                    Oracle.ManagedDataAccess.Client.OracleConfiguration.NamesDirectoryPath = namesDirectoryPath;
            }
            else if (!string.IsNullOrEmpty(value))
                Oracle.ManagedDataAccess.Client.OracleConfiguration.NamesDirectoryPath = value;
        }
    }
    public string TnsAdmin
    {
        get => Oracle.ManagedDataAccess.Client.OracleConfiguration.TnsAdmin;
        set
        {
            bool.TryParse(Environment.GetEnvironmentVariable("DOCKER_RUNNING"), out var dockerRunning);
            if (dockerRunning)
            {
                var tnsAdmin = Environment.GetEnvironmentVariable("ORACLE_TNS_ADMIN");
                if (!string.IsNullOrEmpty(tnsAdmin))
                    Oracle.ManagedDataAccess.Client.OracleConfiguration.TnsAdmin = tnsAdmin;
            }
            else if (!string.IsNullOrEmpty(value))
                Oracle.ManagedDataAccess.Client.OracleConfiguration.TnsAdmin = value;
        }
    }
    public bool ShowSql { get; set; } = false;
}