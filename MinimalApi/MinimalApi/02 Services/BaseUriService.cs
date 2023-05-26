using MinimalApi.Endpoints;

namespace MinimalApi.Services;

public class BaseUriService
{
    private readonly UnitOfWorkService _uow;

    public BaseUriService(UnitOfWorkService uow)
    {
        _uow = uow;
    }

    public async Task<string> GetBaseUriAsync(BaseUriRequest request)
    {
        return await _uow.WebApiVersionRepo.GetBaseUriAsync(request);
    }
}
