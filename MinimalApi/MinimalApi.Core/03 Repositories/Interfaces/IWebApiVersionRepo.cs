using MinimalApi.Shared;

namespace MinimalApi.Core;

public interface IWebApiVersionRepo
{
    Task<string> GetBaseUriAsync(BaseUriRequest request);
}
