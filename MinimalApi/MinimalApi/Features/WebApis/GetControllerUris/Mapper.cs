namespace MinimalApi.Api.Features.WebApis;

public static class Mapper
{
    public static GetControllerUrisResponse MapToControllerUrisResponse(ControllerUriInfoByApplicationDto dto)
    {
        return
            new GetControllerUrisResponse(
                Address: dto.Address,
                Order: dto.Order,
                Port: dto.Port,
                UriName: dto.UriName,
                UseHttps: dto.UseHttps,
                Version: dto.Version);
    }

    public static GetControllerUrisResponse MapToControllerUrisResponse(ControllerUriFacilityInfoByApplicationDto dto)
    {
        return
            new GetControllerUrisResponse(
                Address: dto.Address,                 
                Order: dto.Order,
                Port: dto.Port,
                UriName: dto.UriName,
                UseHttps: dto.UseHttps,
                Version: dto.Version);
    }
}
