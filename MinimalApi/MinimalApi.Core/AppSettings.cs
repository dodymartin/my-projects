using System.ComponentModel.DataAnnotations;

namespace MinimalApi.App;

public class AppSettings
{
    public const string ConfigurationSection = "MinimalApi.App.AppSettings";

    public required string BaseUri { get; set; }
    public required string DatabaseName { get; set; }
    public required string EnvironmentType { get; set; }
    public required bool LogCallers { get; set; } = true;
}