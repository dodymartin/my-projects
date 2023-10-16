using MinimalApi.Shared;

namespace MinimalApi.App.Interfaces;

public interface IWebApiVersionRepo
{
    Task<string?> GetBaseUriAsync(BaseUriRequest request);
}
