namespace MinimalApi.App;

public class AppSettings
{
    public string BaseUri { get; set; }
    public string DatabaseName { get; set; }
    public string EnvironmentType { get; set; }
    public bool LogCallers { get; set; }
}