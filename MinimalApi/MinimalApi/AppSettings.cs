namespace MinimalApi.Api;

public class AppSettings
{
    public const string ConfigurationSection = "MinimalApi.Api.AppSettings";

    public required string BaseUri { get; set; }
    public required string DatabaseName { get; set; }
    public required string EnvironmentType { get; set; }
    public required bool LogCallers { get; set; } = true;
}