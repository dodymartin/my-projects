using MinimalApi.Api.Features.WebApis;
using NSubstitute;

namespace MinimalApi.Api.Tests.IntegrationTests.WebApis.Features.GetBaseUri;

public class GetBaseUriQueryHandlerTests
{
    private IWebApiRepo _mockRepo;
    private GetBaseUriQueryHandler _handler;

    public GetBaseUriQueryHandlerTests()
    {
        _mockRepo = Substitute.For<IWebApiRepo>();
        _handler = new GetBaseUriQueryHandler(_mockRepo);
    }

    [Fact]
    public async Task GetBaseUriQuery_FoundApplication_ReturnBaseUri()
    {
        // Arrange
        var queryRequest = new GetBaseUriQuery(
            ApplicationId: 252,
            ApplicationVersion: "5.2"
        );

        var dto = new WebApiVersionDto
        {
            ApplicationId = queryRequest.ApplicationId!.Value,
            Port = 1234,
            UseHttps = false,
            Version = queryRequest.ApplicationVersion!,
            WebApiId = 1
        };

        _mockRepo.GetOneVersionAsync(queryRequest.ApplicationId!.Value, queryRequest.ApplicationVersion!, CancellationToken.None)
            .Returns(dto);

        // Act
        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsError);
            Assert.True(result.Value == dto.GetBaseUri());
        });
    }

    [Fact]
    public async Task GetBaseUriQuery_NotFoundApplication_ReturnSingleError()
    {
        // Arrange
        var queryRequest = new GetBaseUriQuery(
            ApplicationId: 9999,
            ApplicationVersion: "0.0"
        );

        _mockRepo.GetOneVersionAsync(queryRequest.ApplicationId!.Value, queryRequest.ApplicationVersion!, CancellationToken.None)
            .Returns(default(WebApiVersionDto));

        // Act
        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsError);
            Assert.NotEmpty(result.Errors);
            Assert.Single(result.Errors);
        });
    }
}
