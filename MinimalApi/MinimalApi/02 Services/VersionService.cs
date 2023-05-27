using MinimalApi.Endpoints;

namespace MinimalApi.Services;

public class VersionService
{
    private readonly UnitOfWorkService _uow;

    public VersionService(UnitOfWorkService uow)
    {
        _uow = uow;
    }

    public async Task<bool> CheckMinimumVersionAsync(VersionCheckMinimumRequest request)
    {
        var apln = await _uow.ApplicationRepo.GetApplicationAsync(request.ApplicationId, request.ApplicationName);
        if (apln is null)
            return await Task.FromResult(false);

        string minimumVersion;
        if (request.FacilityId.HasValue)
            minimumVersion = await _uow.ApplicationFacilityRepo.GetMinimumVersionAsync(apln.Id, request.FacilityId.Value);
        else
            minimumVersion = await _uow.ApplicationRepo.GetMinimumVersionAsync(apln.Id);

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
