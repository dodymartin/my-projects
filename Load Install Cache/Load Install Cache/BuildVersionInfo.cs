namespace Load_Install_Cache;

public class BuildVersionInfo
{
    public static string FileName = "_buildVersionInfo.json";

    public string ApplicationExeName { get; set; }
    public string ApplicationName { get; set; }
    public DateTime BuildDateTime { get; set; }
    public string BuildType { get; set; }
    public string BuildUserId { get; set; }
    public string BuildUserName { get; set; }
    public bool IsPublished { get; set; }
    public string Version { get; set; }

    public override string ToString() => $"{Version}, {BuildDateTime}, {BuildUserId}, \"{BuildUserName}\"{(IsPublished ? " - (Published)" : string.Empty)}";
}