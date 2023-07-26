namespace Load_Install_Cache;

public class CacheApplication
{
    public string ExeName { get; set; }
    public string Name { get; set; }
    public IList<BuildVersionInfo> Versions { get; set; }
        = new List<BuildVersionInfo>();
}
