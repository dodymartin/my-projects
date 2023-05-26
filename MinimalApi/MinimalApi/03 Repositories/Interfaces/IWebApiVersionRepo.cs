using MinimalApi.Endpoints;

namespace MinimalApi.Repositories;

public interface IWebApiVersionRepo
{
    Task<string> GetBaseUriAsync(BaseUriRequest request);
}
