using ErrorOr;
using MediatR;
using MinimalApi.App.Interfaces;

namespace MinimalApi.App.Queries.WebApis;

public class GetBaseUriQueryHandler : IRequestHandler<GetBaseUriQuery, ErrorOr<string?>>
{
    private readonly IWebApiRepo _webApiRepo;

    public GetBaseUriQueryHandler(IWebApiRepo webApiRepo)
    {
        _webApiRepo = webApiRepo;
    }

    public async Task<ErrorOr<string?>> Handle(GetBaseUriQuery queryRequest, CancellationToken cancellationToken)
    {
        var webApiVersionDto = await _webApiRepo.GetOneVersionAsync(queryRequest.ApplicationId!.Value, queryRequest.ApplicationVersion!, cancellationToken);
        if (webApiVersionDto is not null)
        {
            return webApiVersionDto.GetBaseUri();
        }
        return Error.NotFound($"Web Api version {queryRequest.ApplicationVersion} is not found.");
    }
}
