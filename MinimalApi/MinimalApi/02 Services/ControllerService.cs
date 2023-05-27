using Microsoft.Extensions.Options;
using MinimalApi.Dal;
using MinimalApi.Endpoints;

namespace MinimalApi.Services;

public class ControllerService
{
    private readonly UnitOfWorkService _uow;
    private readonly AppSettings _appSettings;

    public ControllerService(UnitOfWorkService uow, IOptions<AppSettings> appSettings)
    {
        _uow = uow;
        _appSettings = appSettings.Value;
    }

    public async Task<IList<ControllerUriResponse>> GetControllerUriInfoAsync(ControllerUriRequest request)
    {
        var apln = await _uow.ApplicationRepo.GetApplicationAsync(request.ApplicationId, request.ApplicationName);
        if (apln is null)
        {
            if (request.ApplicationId.HasValue)
                throw new Exception($"ApplicationId ({request.ApplicationId.Value}) is not found in cmn_mstr.apln");
            throw new Exception($"ApplicationName ({request.ApplicationName}) is not found in cmn_mstr.apln");
        }

        var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), _appSettings.EnvironmentType);

        if (request.FacilityId.HasValue)
        {
            return
                (await _uow.ControllerUriFacilityInfoByApplicationRepo.GetControllerUrisAsync(environmentType, request.ControllerName, apln.Id, request.ApplicationVersion, request.FacilityId.Value))
                .OrderBy(x => x.Order)
                .Select(x => (ControllerUriResponse)x)
                .ToList();
        }
        else if (!string.IsNullOrEmpty(request.MachineName))
        {
            return
                (await _uow.ControllerUriInfoByApplicationRepo.GetControllerUrisAsync(environmentType, request.ControllerName, apln.Id, request.ApplicationVersion, request.MachineName))
                .OrderBy(x => x.Order)
                .Select(x => (ControllerUriResponse)x)
                .ToList();
        }
        else
        {
            return
                (await _uow.ControllerUriInfoByApplicationRepo.GetControllerUrisAsync(environmentType, request.ControllerName, apln.Id, request.ApplicationVersion))
                .OrderBy(x => x.Order)
                .Select(x => (ControllerUriResponse)x)
                .ToList();
        }
    }
}
