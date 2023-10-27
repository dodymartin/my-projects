
namespace MinimalApi.Api.Features.WebApis;

public record GetControllerUrisResponse(
    string Address,
    int Order,
    int Port,
    string UriName,
    bool UseHttps,
    string Version);