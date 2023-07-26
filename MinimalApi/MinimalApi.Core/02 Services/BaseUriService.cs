using MinimalApi.Shared;

namespace MinimalApi.Core;

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
