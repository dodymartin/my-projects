//using Microsoft.Extensions.Options;
//using MinimalApi.App.Interfaces;
//using MinimalApi.Dom.Enumerations;
//using MinimalApi.Dom.Facilities.ValueObjects;
//using MinimalApi.Shared;
//using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

//namespace MinimalApi.App;

//public class ControllerService
//{
//    private readonly AppSettings _appSettings;
//    private readonly IApplicationRepo _applicationRepo;
//    private readonly IControllerUriInfoByApplicationRepo _controllerUriInfoByApplicationRepo;
//    private readonly IControllerUriFacilityInfoByApplicationRepo _controllerUriFacilityInfoByApplicationRepo;

//    public ControllerService(IOptions<AppSettings> appSettings,
//        IApplicationRepo applicationRepo,
//        IControllerUriInfoByApplicationRepo controllerUriInfoByApplicationRepo,
//        IControllerUriFacilityInfoByApplicationRepo controllerUriFacilityInfoByApplicationRepo)
//    {
//        _appSettings = appSettings.Value;
//        _applicationRepo = applicationRepo;
//        _controllerUriInfoByApplicationRepo = controllerUriInfoByApplicationRepo;
//        _controllerUriFacilityInfoByApplicationRepo = controllerUriFacilityInfoByApplicationRepo;
//    }

//    public async Task<IList<ControllerUriResponse>> GetControllerUriInfoAsync(ControllerUriRequest request)
//    {
//        return await Task.FromResult(new List<ControllerUriResponse>());
//        //var applicationId = default(ApplicationId);
//        //if (request.ApplicationId is not null)
//        //    applicationId = ApplicationId.Create(request.ApplicationId.Value);

//        //var apln = await _applicationRepo.GetApplicationAsync(applicationId.Value, request.ApplicationName, new CancellationToken());
//        //if (apln is null)
//        //{
//        //    if (request.ApplicationId.HasValue)
//        //        throw new Exception($"ApplicationId ({request.ApplicationId.Value}) is not found in cmn_mstr.apln");
//        //    throw new Exception($"ApplicationName ({request.ApplicationName}) is not found in cmn_mstr.apln");
//        //}

//        //var environmentType = (EnvironmentTypes)Enum.Parse(typeof(EnvironmentTypes), _appSettings.EnvironmentType);

//        //if (request.FacilityId.HasValue)
//        //{
//        //    return
//        //        (await _controllerUriFacilityInfoByApplicationRepo.GetControllerUrisAsync(environmentType, request.ControllerName, ApplicationId.Create(apln.Id.Value), request.ApplicationVersion, FacilityId.Create(request.FacilityId.Value)))
//        //        .OrderBy(x => x.Order)
//        //        .Select(x => (ControllerUriResponse)x)
//        //        .ToList();
//        //}
//        //else if (!string.IsNullOrEmpty(request.MachineName))
//        //{
//        //    return
//        //        (await _controllerUriInfoByApplicationRepo.GetControllerUrisAsync(environmentType, request.ControllerName, ApplicationId.Create(apln.Id.Value), request.ApplicationVersion, request.MachineName))
//        //        .OrderBy(x => x.Order)
//        //        .Select(x => (ControllerUriResponse)x)
//        //        .ToList();
//        //}
//        //else
//        //{
//        //    return
//        //        (await _controllerUriInfoByApplicationRepo.GetControllerUrisAsync(environmentType, request.ControllerName, ApplicationId.Create(apln.Id.Value), request.ApplicationVersion))
//        //        .OrderBy(x => x.Order)
//        //        .Select(x => (ControllerUriResponse)x)
//        //        .ToList();
//        //}
//    }
//}
