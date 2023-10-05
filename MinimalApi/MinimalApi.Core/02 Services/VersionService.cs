using MinimalApi.Shared;

namespace MinimalApi.Core;

public class VersionService
{
    private readonly IApplicationRepo _applicationRepo;
    private readonly IApplicationFacilityRepo _applicationFacilityRepo;

    public VersionService(IApplicationRepo applicationRepo, IApplicationFacilityRepo applicationFacilityRepo)
    {
        _applicationRepo = applicationRepo;
        _applicationFacilityRepo = applicationFacilityRepo;
    }

    public async Task<bool> CheckMinimumVersionAsync(VersionCheckMinimumRequest request)
    {
        var applicationId = default(ApplicationId);
        if (!request.ApplicationId.HasValue)
            applicationId = new ApplicationId(request.ApplicationId.Value);

        var apln = await _applicationRepo.GetApplicationAsync(applicationId, request.ApplicationName);
        if (apln is null)
            return await Task.FromResult(false);

        string minimumVersion;
        if (request.FacilityId.HasValue)
            minimumVersion = await _applicationFacilityRepo.GetMinimumVersionAsync(apln.Id, new FacilityId(request.FacilityId.Value));
        else
            minimumVersion = await _applicationRepo.GetMinimumVersionAsync(apln.Id);

        return await Task.FromResult(CheckVersion(minimumVersion, request.ApplicationVersion));
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
