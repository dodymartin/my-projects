using ErrorOr;
using MediatR;
using MinimalApi.App.Interfaces;
using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

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
        var webApiDto = await _webApiRepo.GetOneVersionAsync(queryRequest.ApplicationId!.Value, queryRequest.ApplicationVersion!, cancellationToken);
        if (webApiDto is not null)
        {
            return webApiDto.GetBaseUriByApplicationVersion(webApiDto.UseHttps, queryRequest.ApplicationVersion!);
        }
        return Error.NotFound();
    }
}
