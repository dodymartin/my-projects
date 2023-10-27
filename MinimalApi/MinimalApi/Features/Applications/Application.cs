using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.Applications;

public class Application : AggregateRoot<ApplicationId, int> //EntityBase<Application, ApplicationId>
{
    public string ExeName { get; set; }
    public string FromDirectoryName { get; set; }
    public string? MinimumAssemblyVersion { get; set; }
    public string Name { get; set; }

    private readonly List<ApplicationFacility> _facilities = new();
    public IReadOnlyList<ApplicationFacility> Facilities => _facilities.AsReadOnly();

    private readonly List<ApplicationVersion> _versions = new();
    public IReadOnlyList<ApplicationVersion> Versions => _versions.AsReadOnly();

    public static bool CheckVersion(string? minimumVersion, string version)
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
}
