using MinimalApi.App.Interfaces;
using MinimalApi.Shared;

namespace MinimalApi.App;

public class BaseUriService
{
    private readonly IWebApiVersionRepo _webApiVersionRepo;

    public BaseUriService(IWebApiVersionRepo webApiVersionRepo)
    {
        _webApiVersionRepo = webApiVersionRepo;
    }

    public async Task<string> GetBaseUriAsync(BaseUriRequest request)
    {
        return await _webApiVersionRepo.GetBaseUriAsync(request);
    }
}
