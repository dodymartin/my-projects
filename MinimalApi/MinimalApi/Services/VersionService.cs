using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Version;

public interface IVersionService
{
    VersionDataContextSettings ContextSettings { get; }

    void WriteCollections();
    bool MinimumVersion(int applicationId, string version);
    bool MinimumVersion(int applicationId, string version, int facilityId);
    bool MinimumVersion(string applicationName, string version);
    bool MinimumVersion(string applicationName, string version, int facilityId);
}

public class VersionService : IVersionService
{
    private readonly ILogger<VersionService> _logger;
    private readonly IVersionDataContext _ctx;

    public VersionDataContextSettings ContextSettings { get; }

    public VersionService(ILogger<VersionService> logger, IVersionDataContext ctx, VersionDataContextSettings contextSettings)
    {
        _logger = logger;
        _ctx = ctx;
        ContextSettings = contextSettings;
    }

    public void WriteCollections()
    {
        foreach (var application in _ctx.Applications)
            _logger.LogInformation(application.Name);
        foreach (var applicationFacility in _ctx.ApplicationFacilities
            .Include(a => a.Application)
            .Include(f => f.Facility))
            _logger.LogInformation($"{applicationFacility.Application.Name}, {applicationFacility.Facility.Name}");
    }

    public bool MinimumVersion(int applicationId, string version)
    {
        var minimumVersion =
            (from a in _ctx.Applications
             where (int)a.Id == applicationId
             select a.MinimumAssemblyVersion)
            .FirstOrDefault(); ;
        return CheckVersion(minimumVersion, version);
    }

    public bool MinimumVersion(int applicationId, string version, int facilityId)
    {
        var minimumVersion =
            (from af in _ctx.ApplicationFacilities
             where (int)af.Application.Id == applicationId
                && af.Facility.Id == facilityId
             select af.MinimumAssemblyVersion)
            .FirstOrDefault(); ;
        return CheckVersion(minimumVersion, version);
    }

    public bool MinimumVersion(string applicationName, string version)
    {
        var minimumVersion =
            (from a in _ctx.Applications
             where a.Name == applicationName
             select a.MinimumAssemblyVersion)
            .FirstOrDefault();
        return CheckVersion(minimumVersion, version);
    }

    public bool MinimumVersion(string applicationName, string version, int facilityId)
    {
        var minimumVersion =
            (from af in _ctx.ApplicationFacilities
             where af.Application.Name == applicationName
                && af.Facility.Id == facilityId
             select af.MinimumAssemblyVersion)
            .FirstOrDefault();
        return CheckVersion(minimumVersion, version);
    }

    #region Private Methods

    private static bool CheckVersion(string minimumVersion, string version)
    {
        if (!string.IsNullOrEmpty(minimumVersion))
        {
            var versionParts = version.Split('.');
            var minimumVersionParts = minimumVersion.Split('.');
            for (var i = 0; i < minimumVersionParts.Length; i++)
            {
                if (int.TryParse(minimumVersionParts[i], out var minimumVersionPart))
                {
                    var versionPart = 0;
                    if (i < versionParts.Length)
                        if (!int.TryParse(versionParts[i], out versionPart))
                            throw new Exception("Version Parts must be numeric.");

                    if (versionPart < minimumVersionPart)
                        return false;
                    else if (versionPart > minimumVersionPart)
                        return true;
                }
                else
                    throw new Exception("Minimum Version Parts must be numeric.");
            }
        }
        return true;
    }

    #endregion
}
